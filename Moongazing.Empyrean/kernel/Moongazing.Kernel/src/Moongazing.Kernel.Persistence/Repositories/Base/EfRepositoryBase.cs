using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Moongazing.Kernel.Persistence.Dynamic;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Persistence.Repositories.Common;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Moongazing.Kernel.Persistence.Repositories.Base;

public class EfRepositoryBase<TEntity, TEntityId, TContext> : IAsyncRepository<TEntity, TEntityId>, IRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TContext : DbContext
{
    protected readonly TContext Context;

    public EfRepositoryBase(TContext context)
    {
        Context = context;
    }
    /// <summary>
    /// Provides a queryable object for the current entity.
    /// </summary>
    /// <returns>A queryable object for the entity type.</returns>
    public IQueryable<TEntity> Query()
    {
        return Context.Set<TEntity>();
    }

    /// <summary>
    /// Updates the properties of the entity before adding it to the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    protected virtual void EditEntityPropertiesToAdd(TEntity entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
    }
    /// <summary>
    /// Adds a single entity to the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>The added entity.</returns>
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        EditEntityPropertiesToAdd(entity);
        await Context.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    /// <summary>
    /// Adds a collection of entities to the database asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to add.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>The collection of added entities.</returns>
    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities,
                                                          CancellationToken cancellationToken = default)
    {
        foreach (TEntity entity in entities)
        {
            EditEntityPropertiesToAdd(entity);
        }
        await Context.AddRangeAsync(entities, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entities;
    }
    /// <summary>
    /// Updates the properties of the entity before updating it in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    protected virtual void EditEntityPropertiesToUpdate(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
    }
    /// <summary>
    /// Updates a single entity in the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>The updated entity.</returns>
    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        EditEntityPropertiesToUpdate(entity);
        Context.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    /// <summary>
    /// Updates a collection of entities in the database asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to update.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>The collection of updated entities.</returns>
    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities,
                                                             CancellationToken cancellationToken = default)
    {
        foreach (TEntity entity in entities)
        {
            EditEntityPropertiesToUpdate(entity);
        }
        Context.UpdateRange(entities);
        await Context.SaveChangesAsync(cancellationToken);
        return entities;
    }
    /// <summary>
    /// Deletes a single entity from the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="permanent">Indicates whether the deletion is permanent or soft.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>The deleted entity.</returns>
    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, CancellationToken cancellationToken = default)
    {
        await SetEntityAsDeleted(entity, permanent, isAsync: true, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    /// <summary>
    /// Deletes a collection of entities from the database asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to delete.</param>
    /// <param name="permanent">Indicates whether the deletion is permanent or soft.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>The collection of deleted entities.</returns>
    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities,
                                                             bool permanent = false,
                                                             CancellationToken cancellationToken = default)
    {
        await SetEntityAsDeleted(entities, permanent, isAsync: true, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entities;
    }
    /// <summary>
    /// Retrieves a paginated list of entities from the database asynchronously.
    /// </summary>
    /// <param name="predicate">The filter to apply to the query.</param>
    /// <param name="orderBy">The order to apply to the query.</param>
    /// <param name="include">Related entities to include in the query.</param>
    /// <param name="index">The page index to retrieve.</param>
    /// <param name="size">The size of each page.</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities.</param>
    /// <param name="enableTracking">Indicates whether to enable EF Core tracking.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>A paginated list of entities.</returns>
    public async Task<IPagebale<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                       int index = 0,
                                                       int size = 10,
                                                       bool withDeleted = false,
                                                       bool enableTracking = true,
                                                       CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }
    /// <summary>
    /// Retrieves a single entity from the database asynchronously based on a predicate.
    /// </summary>
    /// <param name="predicate">The filter to apply to the query.</param>
    /// <param name="include">Related entities to include in the query.</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities.</param>
    /// <param name="enableTracking">Indicates whether to enable EF Core tracking.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>The entity matching the predicate, or null if not found.</returns>
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                         bool withDeleted = false,
                                         bool enableTracking = true,
                                         CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
        {
            queryable = queryable.AsNoTracking();
        }
        if (include != null)
        {
            queryable = include(queryable);
        }
        if (withDeleted)
        {
            queryable = queryable.IgnoreQueryFilters();
        }
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    /// <summary>
    /// Retrieves all entities from the database asynchronously with optional filtering, ordering, and including related entities.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities. If null, retrieves all entities.</param>
    /// <param name="orderBy">A function to order the entities.</param>
    /// <param name="include">A function to include related entities in the query.</param>
    /// <param name="withDeleted">If true, includes soft-deleted entities in the query.</param>
    /// <param name="enableTracking">If false, disables EF Core's change tracking for better performance.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The result is a list of entities matching the specified conditions.</returns>
    public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                  bool withDeleted = false,
                                                  bool enableTracking = true,
                                                  CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();

        if (!enableTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        if (include != null)
        {
            queryable = include(queryable);
        }

        if (withDeleted)
        {
            queryable = queryable.IgnoreQueryFilters();
        }

        if (predicate != null)
        {
            queryable = queryable.Where(predicate);
        }

        if (orderBy != null)
        {
            queryable = orderBy(queryable);
        }

        return await queryable.ToListAsync(cancellationToken);
    }
    /// <summary>
    /// Checks if any entity matches a specified condition asynchronously.
    /// </summary>
    /// <param name="predicate">The condition to check.</param>
    /// <param name="include">Related entities to include in the query.</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>True if any entity matches the condition; otherwise, false.</returns>
    public async Task<IPagebale<TEntity>> GetListByDynamicAsync(DynamicQuery dynamic,
                                                                Expression<Func<TEntity, bool>>? predicate = null,
                                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                                int index = 0,
                                                                int size = 10,
                                                                bool withDeleted = false,
                                                                bool enableTracking = true,
                                                                CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
        {
            queryable = queryable.AsNoTracking();
        }
        if (include != null)
        {
            queryable = include(queryable);
        }
        if (withDeleted)
        {
            queryable = queryable.IgnoreQueryFilters();
        }
        if (predicate != null)
        {
            queryable = queryable.Where(predicate);
        }
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }
    /// <summary>
    /// Checks asynchronously whether any entity in the database matches the specified predicate.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities. If null, checks for any entity.</param>
    /// <param name="include">A function to include related entities in the query.</param>
    /// <param name="withDeleted">If true, includes soft-deleted entities in the query.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The result is true if any entity matches the condition; otherwise, false.</returns>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                     bool withDeleted = false,
                                     CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (withDeleted)
        {
            queryable = queryable.IgnoreQueryFilters();
        }
        if (predicate != null)
        {
            queryable = queryable.Where(predicate);
        }
        return await queryable.AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a single entity to the database and saves changes immediately.
    /// </summary>
    /// <param name="entity">The entity to be added to the database.</param>
    /// <returns>The added entity after being saved to the database.</returns>
    public TEntity Add(TEntity entity)
    {
        EditEntityPropertiesToAdd(entity);
        Context.Add(entity);
        Context.SaveChanges();
        return entity;
    }
    /// <summary>
    /// Adds multiple entities to the database and saves changes immediately.
    /// </summary>
    /// <param name="entities">The collection of entities to be added to the database.</param>
    /// <returns>The collection of added entities after being saved to the database.</returns>
    public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
    {
        foreach (TEntity entity in entities)
        {
            EditEntityPropertiesToAdd(entity);
        }
        Context.AddRange(entities);
        Context.SaveChanges();
        return entities;
    }
    /// <summary>
    /// Updates an existing entity in the database and saves changes immediately.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>The updated entity after being saved to the database.</returns>
    public TEntity Update(TEntity entity)
    {
        EditEntityPropertiesToAdd(entity);
        Context.Update(entity);
        Context.SaveChanges();
        return entity;
    }
    /// <summary>
    /// Updates multiple entities in the database and saves changes immediately.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    /// <returns>The collection of updated entities after being saved to the database.</returns>
    public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
    {
        foreach (TEntity entity in entities)
        {
            EditEntityPropertiesToAdd(entity);
        }
        Context.UpdateRange(entities);
        Context.SaveChanges();
        return entities;
    }
    /// <summary>
    /// Deletes a single entity from the database. Supports both permanent and soft deletion.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <param name="permanent">If true, performs a permanent deletion; otherwise, performs a soft deletion.</param>
    /// <returns>The deleted entity after changes are saved to the database.</returns>
    public TEntity Delete(TEntity entity, bool permanent = false)
    {
        SetEntityAsDeleted(entity, permanent, isAsync: false).Wait();
        Context.SaveChanges();
        return entity;
    }
    /// <summary>
    /// Deletes multiple entities from the database. Supports both permanent and soft deletion.
    /// </summary>
    /// <param name="entities">The collection of entities to be deleted.</param>
    /// <param name="permanent">If true, performs a permanent deletion; otherwise, performs a soft deletion.</param>
    /// <returns>The collection of deleted entities after changes are saved to the database.</returns>
    public ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false)
    {
        SetEntityAsDeleted(entities, permanent, isAsync: false).Wait();
        Context.SaveChanges();
        return entities;
    }
    /// <summary>
    /// Retrieves a single entity from the database that matches the given predicate.
    /// </summary>
    /// <param name="predicate">The condition to filter the entity.</param>
    /// <param name="include">Related entities to include in the query.</param>
    /// <param name="withDeleted">If true, includes soft-deleted entities in the query.</param>
    /// <param name="enableTracking">If false, disables EF Core tracking for performance improvement.</param>
    /// <returns>The entity matching the predicate, or null if not found.</returns>
    public TEntity? Get(Expression<Func<TEntity, bool>> predicate,
                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                        bool withDeleted = false,
                        bool enableTracking = true)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
        {
            queryable = queryable.AsNoTracking();
        }
        if (include != null)
        {
            queryable = include(queryable);
        }
        if (withDeleted)
        {
            queryable = queryable.IgnoreQueryFilters();
        }
        return queryable.FirstOrDefault(predicate);
    }
    /// <summary>
    /// Retrieves a paginated list of entities from the database.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities. If null, retrieves all entities.</param>
    /// <param name="orderBy">A function to order the entities.</param>
    /// <param name="include">Related entities to include in the query.</param>
    /// <param name="index">The page index to retrieve.</param>
    /// <param name="size">The size of each page.</param>
    /// <param name="withDeleted">If true, includes soft-deleted entities in the query.</param>
    /// <param name="enableTracking">If false, disables EF Core tracking for performance improvement.</param>
    /// <returns>A paginated list of entities matching the specified conditions.</returns>
    public IPagebale<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null,
                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                      int index = 0,
                                      int size = 10,
                                      bool withDeleted = false,
                                      bool enableTracking = true)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return orderBy(queryable).ToPaginate(index, size);
        return queryable.ToPaginate(index, size);
    }


    /// <summary>
    /// Retrieves a paginated list of entities using a dynamic query.
    /// </summary>
    /// <param name="dynamic">The dynamic query to apply.</param>
    /// <param name="predicate">The condition to filter the entities. If null, retrieves all entities.</param>
    /// <param name="include">Related entities to include in the query.</param>
    /// <param name="index">The page index to retrieve.</param>
    /// <param name="size">The size of each page.</param>
    /// <param name="withDeleted">If true, includes soft-deleted entities in the query.</param>
    /// <param name="enableTracking">If false, disables EF Core tracking for performance improvement.</param>
    /// <returns>A paginated list of entities matching the specified conditions and dynamic query.</returns>
    public IPagebale<TEntity> GetListByDynamic(DynamicQuery dynamic,
                                               Expression<Func<TEntity, bool>>? predicate = null,
                                               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                               int index = 0,
                                               int size = 10,
                                               bool withDeleted = false,
                                               bool enableTracking = true)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.ToPaginate(index, size);
    }
    /// <summary>
    /// Checks if any entity matches the specified condition in the database.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities. If null, checks for any entity.</param>
    /// <param name="include">Related entities to include in the query.</param>
    /// <param name="withDeleted">If true, includes soft-deleted entities in the query.</param>
    /// <returns>True if any entity matches the condition; otherwise, false.</returns>
    public bool Any(Expression<Func<TEntity, bool>>? predicate = null,
                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                    bool withDeleted = false)
    {
        IQueryable<TEntity> queryable = Query();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.Any();
    }

    /// <summary>
    /// Marks an entity as deleted. Supports both permanent deletion and soft deletion.
    /// </summary>
    /// <param name="entity">The entity to be marked as deleted.</param>
    /// <param name="permanent">If true, the entity will be permanently deleted; otherwise, it will be soft deleted.</param>
    /// <param name="isAsync">Indicates whether the operation should be performed asynchronously.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    protected async Task SetEntityAsDeleted(TEntity entity,
                                            bool permanent,
                                            bool isAsync = true,
                                            CancellationToken cancellationToken = default)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            if (isAsync)
                await SetEntityAsSoftDeleted(entity, isAsync, cancellationToken: cancellationToken);
            else
                SetEntityAsSoftDeleted(entity, isAsync).Wait(cancellationToken);
        }
        else
            Context.Remove(entity);
    }
    /// <summary>
    /// Marks a collection of entities as deleted. Supports both permanent deletion and soft deletion.
    /// </summary>
    /// <param name="entities">The collection of entities to be marked as deleted.</param>
    /// <param name="permanent">If true, the entities will be permanently deleted; otherwise, they will be soft deleted.</param>
    /// <param name="isAsync">Indicates whether the operation should be performed asynchronously.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    protected async Task SetEntityAsDeleted(IEnumerable<TEntity> entities,
                                            bool permanent,
                                            bool isAsync = true,
                                            CancellationToken cancellationToken = default)
    {
        foreach (TEntity entity in entities)
            await SetEntityAsDeleted(entity, permanent, isAsync, cancellationToken);
    }
    /// <summary>
    /// Constructs a query to load related entities while excluding soft-deleted entities.
    /// </summary>
    /// <param name="query">The base query to load related entities.</param>
    /// <param name="navigationPropertyType">The type of the navigation property being queried.</param>
    /// <returns>A queryable object that includes related entities and excludes soft-deleted entities.</returns>
    protected IQueryable<object>? GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        Type queryProviderType = query.Provider.GetType();
        MethodInfo createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery = (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: [query.Expression])!;
        return queryProviderQuery.Where(x => !((IEntityTimestampsMetadata)x).DeletedDate.HasValue);
    }
    /// <summary>
    /// Validates whether the entity has a one-to-one relationship that could be affected by a soft deletion.
    /// </summary>
    /// <param name="entity">The entity to check for one-to-one relationships.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the entity has a one-to-one relationship and soft deletion might cause issues with foreign keys.
    /// </exception>
    protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        IEnumerable<IForeignKey> foreignKeys = Context.Entry(entity).Metadata.GetForeignKeys();
        IForeignKey? oneToOneForeignKey = foreignKeys.FirstOrDefault(fk => fk.IsUnique
        && fk.PrincipalKey.Properties.All(pk => Context.Entry(entity).Property(pk.Name).Metadata.IsPrimaryKey())
        );

        if (oneToOneForeignKey != null)
        {
            string relatedEntity = oneToOneForeignKey.PrincipalEntityType.ClrType.Name;
            IReadOnlyList<IProperty> primaryKeyProperties = Context.Entry(entity).Metadata.FindPrimaryKey()!.Properties;
            string primaryKeyNames = string.Join(", ", primaryKeyProperties.Select(prop => prop.Name));
            throw new InvalidOperationException(
                $"Entity {entity.GetType().Name} has a one-to-one relationship with {relatedEntity} via the primary key ({primaryKeyNames}). Soft Delete causes problems if you try to create an entry again with the same foreign key."
            );
        }
    }
    /// <summary>
    /// Updates the entity's properties to mark it as soft deleted.
    /// </summary>
    /// <param name="entity">The entity to be updated for soft deletion.</param>
    protected virtual void EditEntityPropertiesToDelete(TEntity entity)
    {
        entity.DeletedDate = DateTime.UtcNow;
    }
    /// <summary>
    /// Updates related entity properties for cascading soft deletion in case of navigation properties.
    /// </summary>
    /// <param name="entity">The related entity to be updated for cascading soft deletion.</param>
    protected virtual void EditRelationEntityPropertiesToCascadeSoftDelete(IEntityTimestampsMetadata entity)
    {
        entity.DeletedDate = DateTime.UtcNow;
    }
    /// <summary>
    /// Checks whether an entity is marked as soft deleted.
    /// </summary>
    /// <param name="entity">The entity to check for soft deletion status.</param>
    /// <returns>True if the entity is soft deleted; otherwise, false.</returns>
    protected virtual bool IsSoftDeleted(IEntityTimestampsMetadata entity)
    {
        return entity.DeletedDate.HasValue;
    }
    /// <summary>
    /// Marks an entity and its related entities as soft deleted recursively.
    /// </summary>
    /// <param name="entity">The root entity to mark as soft deleted.</param>
    /// <param name="isAsync">Indicates whether the operation should be performed asynchronously.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <param name="isRoot">Indicates whether the current entity is the root of the soft delete operation.</param>
    private async Task SetEntityAsSoftDeleted(IEntityTimestampsMetadata entity,
                                          bool isAsync = true,
                                          bool isRoot = true,
                                          CancellationToken cancellationToken = default)
    {
        if (IsSoftDeleted(entity))
            return;
        if (isRoot)
            EditEntityPropertiesToDelete((TEntity)entity);
        else
            EditRelationEntityPropertiesToCascadeSoftDelete(entity);

        var navigations = Context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x =>
                x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade }
            )
            .ToList();
        foreach (INavigation? navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object? navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = Context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();

                    if (isAsync)
                    {
                        IQueryable<object>? relationLoaderQuery = GetRelationLoaderQuery(
                            query,
                            navigationPropertyType: navigation.PropertyInfo.GetType()
                        );
                        if (relationLoaderQuery is not null)
                            navValue = await relationLoaderQuery.ToListAsync(cancellationToken);
                    }
                    else
                        navValue = GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                            ?.ToList();

                    if (navValue == null)
                        continue;
                }

                foreach (object navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeleted((IEntityTimestampsMetadata)navValueItem, isAsync, isRoot: false, cancellationToken);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = Context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();

                    if (isAsync)
                    {
                        IQueryable<object>? relationLoaderQuery = GetRelationLoaderQuery(query,
                                                                                         navigationPropertyType: navigation.PropertyInfo.GetType());
                        if (relationLoaderQuery is not null)
                        {
                            navValue = await relationLoaderQuery.FirstOrDefaultAsync(cancellationToken);
                        }
                    }
                    else
                        navValue = GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())?.FirstOrDefault();

                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeleted((IEntityTimestampsMetadata)navValue, isAsync, isRoot: false, cancellationToken);
            }
        }

        Context.Update(entity);
    }
}