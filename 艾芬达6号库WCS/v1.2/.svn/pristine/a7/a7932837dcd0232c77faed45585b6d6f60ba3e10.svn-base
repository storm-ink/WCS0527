/*
 * ================================================
 * 创建人：王建军
 * 创建日期：2012/12/26
 * 备注：
 * 
 * 修改人：
 * 修改日期：
 * 备注：
 * ================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
using System.Xml;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Text.RegularExpressions;
using Spiral.Base;

public static class Setup
{
    public static bool IsSqlServer()
    {
        return NHibernateHelper.Configuration.Properties["connection.driver_class"].Contains("SqlClientDriver");
    }

    public static bool IsSqlite()
    {
        return NHibernateHelper.Configuration.Properties["connection.driver_class"].Contains("SQLite20Driver");
    }

    public static void ExportSchema()
    {
        SchemaExport export = new SchemaExport(NHibernateHelper.Configuration);

        List<String> list = new List<string>();
        Action<String> scriptAction = script =>
        {
            try
            {
                if (script == null)
                {
                    return;
                }

                if (IsSqlServer())
                {
                    // 归档数据放在另一个文件组
                    if (Regex.IsMatch(script, @"create table .*archived\w+", RegexOptions.IgnoreCase))
                    {
                        script += " on fg_archived_data";
                    }

                    {

                        string pattern = @"create table \[?(?<schema>\w+)\]?\.\[?(?<table>\w+)\]?";
                        Match match = Regex.Match(script, pattern, RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            String pk = match.Result("PK_${schema}_${table}");
                            script = Regex.Replace(script, "primary key", String.Format("constraint {0} primary key", pk), RegexOptions.IgnoreCase);
                        }
                    }
                }

                // 剔除自动生成的 hibernate_hi_values 表
                // 第二个 hibernate_hi_values 是 nh 自动生成的
                if (Regex.IsMatch(script, "create.*hibernate_hi_values") && list.Any(x => Regex.IsMatch(x, "create.*hibernate_hi_values"))
                    || Regex.IsMatch(script, "drop.*hibernate_hi_values") && list.Any(x => Regex.IsMatch(x, "drop.*hibernate_hi_values"))
                    || script.Contains("insert") && script.Contains("hibernate_hi_values")
                    )
                {
                    script = String.Empty;
                }

                if (!String.IsNullOrWhiteSpace(script))
                {
                    list.Add(script);
                }


            }
            catch (Exception ex)
            {

                throw;
            }


        };

        export.Create(scriptAction, false);

        using (NhRepositoryContext context = new NhRepositoryContextFactory().CreateRepositoryContext())
        {
            using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
            {
                ISession session = context.Session;

                //String sql = String.Join(Environment.NewLine + "GO" + Environment.NewLine, list.ToArray());
                foreach (var stmt in list)
                {
                    session.CreateSQLQuery(stmt).ExecuteUpdate();
                }


                unitOfWork.Commit();
            }
        }

        createSupportingData();

    }


    private static void createSupportingData()
    {
        using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
        {
            using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
            {
                ISession session = context.Session;

                #region 为 hilo id 生成策略准备的数据


                var arr = Spiral.NHibernateHelper.GetAllEntityNames();
                foreach (var clazz in arr)
                {
                    HibernateHiValue hi = new HibernateHiValue();
                    hi.clazz = clazz;//.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Last();
                    hi.next_value = 0;
                    session.Save(hi);

                }

                #endregion


                unitOfWork.Commit();

            }
        }

    }


    /// <summary>
    /// 创建数据库的“schema”对象
    /// </summary>
    /// <param name="schemaName"></param>
    public static void CreateSchema(ISession session, String schemaName, String owner)
    {
        int count = session
            .CreateSQLQuery("select Count(1) from sys.schemas where Name = :name")
            .SetParameter<String>("name", schemaName)
            .UniqueResult<Int32>();

        if (count == 0)
        {
            String stmt = String.Format("create schema {0} AUTHORIZATION {1}", schemaName, owner);
            session.CreateSQLQuery(stmt).ExecuteUpdate();
        }

    }

}


