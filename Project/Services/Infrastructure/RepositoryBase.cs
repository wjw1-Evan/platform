using IServices.ISysServices;
using Models;
using Models.Infrastructure;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly DbContext _dataContext;
        private readonly IDbSet<T> _dbset;
        private readonly IUserInfo _userInfo;

        protected RepositoryBase(DbContext databaseFactory, IUserInfo userInfo)
        {
            _dataContext = databaseFactory;
            _userInfo = userInfo;
            _dbset = _dataContext.Set<T>();
        }

        /// <summary>
        /// sql 命令
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>影响的记录数</returns>
        public virtual Task<int> SqlCommandAsync(string sql, params object[] parameters)
        {
            return _dataContext.Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>原类型</returns>
        public virtual DbRawSqlQuery<T> SqlQuery(string sql, params object[] parameters)
        {
            return _dataContext.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// sql查询
        /// </summary>
        /// <typeparam name="T1">返回值类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>新类型</returns>
        public virtual DbRawSqlQuery<T1> SqlQuery<T1>(string sql, params object[] parameters)
        {
            return _dataContext.Database.SqlQuery<T1>(sql, parameters);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            if (entity is IDbSetBase dbSetBase)
            {
                dbSetBase.CreatedBy = _userInfo.UserId;
                entity = dbSetBase as T;
            }

            if (entity is IEnterprise ienterprise && string.IsNullOrEmpty(ienterprise.EnterpriseId))
            {
                ienterprise.EnterpriseId = _userInfo.EnterpriseId;
                entity = ienterprise as T;
            }

            _dbset.Add(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);

            if (entity is IDbSetBase dbSetBase)
            {
                var databaseValues = _dataContext.Entry(dbSetBase).GetDatabaseValues();

                dbSetBase.CreatedBy = databaseValues.GetValue<string>("CreatedBy");
                dbSetBase.CreatedDate = databaseValues.GetValue<string>("CreatedDate");
                dbSetBase.CreatedDateTime = databaseValues.GetValue<DateTimeOffset>("CreatedDateTime");

                dbSetBase.UpdatedBy = _userInfo.UserId;
                dbSetBase.UpdatedDateTime = DateTimeOffset.Now;

                entity = dbSetBase as T;
            }

            if (entity is IEnterprise ienterprise)
            {
                var databaseValues = _dataContext.Entry(ienterprise).GetDatabaseValues();

                var entId = databaseValues.GetValue<string>("EnterpriseId");

                if (!string.IsNullOrEmpty(entId))
                {
                    ienterprise.EnterpriseId = entId;

                    entity = ienterprise as T;
                }
            }

            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 添加或者更新
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <param name="entity">实体</param>
        public virtual void Save(object id, T entity)
        {
            if (id != null)
            {
                Update(entity);
            }
            else
            {
                Add(entity);
            }
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="id">该用户所在企业内数据</param>
        /// <param name="remove">物理删除标记 默认false</param>
        public virtual void Delete(object id, bool remove = false)
        {
            var item = GetById(id);
            Delete(item, remove);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item">全部数据范围</param>
        /// <param name="remove">物理删除标记 默认false</param>
        public virtual void Delete(T item, bool remove = false)
        {
            if (!remove && item is IDbSetBase dbSetBase)//标记删除
                dbSetBase.Deleted = true;
            else
                _dbset.Remove(item);
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="where"></param>
        /// <param name="remove">物理删除标记 默认false</param>
        public virtual void Delete(Expression<Func<T, bool>> where, bool remove = false)
        {
            foreach (var item in GetAll(where))
            {
                Delete(item, remove);
            }
        }

        /// <summary>
        /// 获取单个记录
        /// </summary>
        /// <param name="id">该用户所在企业内</param>
        /// <returns></returns>
        public virtual T GetById(object id)
        {
            var item = _dbset.Find(id);

            if (item is IEnterprise iEnterprise && iEnterprise.EnterpriseId != _userInfo.EnterpriseId) return null;

            return item;
        }

        /// <summary>
        /// 获取符合条件的用户所在企业数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> where)
        {
            return GetAll().Where(where);
        }

        /// <summary>
        /// 获取用户所在企业数据
        /// </summary>
        /// <param name="containsDeleted">包含已删除数据</param>
        /// <param name="allEnt"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll(bool containsDeleted = false, bool allEnt = false)
        {
            var model = _dbset as IQueryable<T>;

            var paramterExpression = Expression.Parameter(typeof(T));


            if (!allEnt && typeof(IEnterprise).IsAssignableFrom(typeof(T)))
            {
                var entpriseLambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Property(paramterExpression, "EnterpriseId"), Expression.Constant(_userInfo.EnterpriseId)), paramterExpression);

                model = model.Where(entpriseLambda);
            }

            if (typeof(IDbSetBase).IsAssignableFrom(typeof(T)))
            {
                if (!containsDeleted)
                {
                    model = model.Where(Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Property(paramterExpression, "Deleted"), Expression.Constant(false)), paramterExpression));//"Deleted=false"
                }

                model = model.OrderByDescending(Expression.Lambda<Func<T, DateTimeOffset>>(Expression.PropertyOrField(paramterExpression, "CreatedDateTime"),
                    paramterExpression));
            }

            if (typeof(IUserDictionary).IsAssignableFrom(typeof(T)))
            {
                model = model.OrderBy(Expression.Lambda<Func<T, string>>(Expression.PropertyOrField(paramterExpression, "SystemId"),
                    paramterExpression));
            }

            return model;
        }

    }

}
