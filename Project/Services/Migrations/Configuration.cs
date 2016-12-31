using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Common;
using Models.SysModels;


namespace Services.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Services.ApplicationDb";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            #region 企业

            var sysEnterprises = new[]
            {
                new SysEnterprise()
                {
                    Id="defalutEnt",
                    EnterpriseName = "系统默认企业"
                },

            };
            context.SysEnterprises.AddOrUpdate(a => new { a.Id }, sysEnterprises);



            #endregion

            #region SysArea

            var sysAreas = new[]
            {
                new SysArea
                {
                    AreaName = "Platform",
                    Name = "管理平台",
                    SystemId = "002"
                }
            };
            context.SysAreas.AddOrUpdate(a => new { a.AreaName, a.Name, a.SystemId }, sysAreas);

            #endregion

            #region SysAction

            var sysActions = new[]
            {
                new SysAction
                {
                    Name = "列表",
                    ActionName = "Index",
                    SystemId = "001",
                    System = true
                },
                new SysAction
                {
                    Name = "详细",
                    ActionName = "Details",
                    SystemId = "003",
                    System = true
                },
                new SysAction
                {
                    Name = "新建",
                    ActionName = "Create",
                    SystemId = "004",
                    System = true
                },
                new SysAction
                {
                    Name = "编辑",
                    ActionName = "Edit",
                    SystemId = "005",
                    System = true
                },
                new SysAction
                {
                    Name = "删除",
                    ActionName = "Delete",
                    SystemId = "006",
                    System = true
                },
                new SysAction
                {
                    Name = "导出",
                    ActionName = "Report",
                    SystemId = "007",
                    System = true
                },
            };
            context.SysActions.AddOrUpdate(a => new { a.ActionName }, sysActions);

            #endregion

            #region SysController

            var sysControllers = new[]
            {

                #region 基础内容 100
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "登陆管理平台",
                    ControllerName = "Index",
                    SystemId = "100",
                    Display = false
                },

                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "桌面",
                    ControllerName = "Desktop",
                    SystemId = "100100",
                    Display = false
                },

                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "使用帮助",
                    ControllerName = "Help",
                    SystemId = "100200",
                    Display = false
                },

                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "消息中心",
                    ControllerName = "MyMessage",
                    SystemId = "100300",
                    Display = false
                },

                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "修改密码",
                    ControllerName = "ChangePassword",
                    SystemId = "100400",
                    Display = false
                },
                    new SysController
                                {
                                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                                    Name = "密码修改",
                                    ControllerName = "SysUserChangePassword",
                                    SystemId = "900500",
                                    Display=false,
                                    Ico = "fa-dot-circle-o"
                                },
                #endregion other                


                #region 用户管理 900
                                new SysController
                                {
                                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                                    Name = "用户管理",
                                    ControllerName = "Index",
                                    SystemId = "900",
                                    Ico = "fa-users",
                                    Display = true
                                },

                                 new SysController
                                {
                                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                                    Name = "组织架构",
                                    ControllerName = "SysDepartment",
                                    SystemId = "900200",
                                    Ico = "fa-dot-circle-o"
                                },

                                new SysController
                                {
                                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                                    Name = "角色管理",
                                    ControllerName = "SysRole",
                                    SystemId = "900300",
                                    Ico = "fa-dot-circle-o"
                                },

                                new SysController
                                {
                                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                                    Name = "用户管理",
                                    ControllerName = "SysUser",
                                    SystemId = "900400",
                                    Ico = "fa-dot-circle-o"
                                },
                                   
             
             
             
                             #endregion

                
                #region 系统开发配置 950
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统设置",
                    ControllerName = "Index",
                    SystemId = "950",
                    Ico="fa-cog"
                },

                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "操作类型",
                    ControllerName = "SysAction",
                    SystemId = "950300",
                    Ico="fa-th-large"
                },

                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统模块",
                    ControllerName = "SysController",
                    SystemId = "950400",
                    Ico="fa-puzzle-piece"
                },

                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "帮助信息",
                    ControllerName = "SysHelp",
                    SystemId = "950900",
                    Ico="fa-info-circle"
                },

                  new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "帮助信息分类",
                    ControllerName = "SysHelpClass",
                    SystemId = "950950",
                    Ico="fa-info-circle"
                },


                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "用户日志",
                    ControllerName = "SysUserLog",
                    SystemId = "950990",
                    Ico = "fa-calendar"
                },

#endregion

            };

            context.SysControllers.AddOrUpdate(
                a => new { a.SysAreaId, a.SystemId }, sysControllers);
            #endregion

            #region SysControllerSysAction

            var sysControllerSysActions = (from sysAction in sysActions.Where(a => a.System)
                                           from sysController in sysControllers
                                           select
                                               new SysControllerSysAction
                                               {
                                                   SysActionId = sysAction.Id,
                                                   SysControllerId = sysController.Id
                                               }).ToArray();

            context.SysControllerSysActions.AddOrUpdate(a => new { a.SysActionId, a.SysControllerId },
                sysControllerSysActions);

            #endregion


            #region 模块特殊Action

            context.SysControllerSysActions.AddOrUpdate(a => new { a.SysActionId, a.SysControllerId }, new SysControllerSysAction[]
            {
                 //new SysControllerSysAction{
                 //   SysActionId = sysActions.First(a=>a.ActionName=="test").Id,
                 //   SysControllerId = sysControllers.First(a=>a.ControllerName== "MyMessage").Id},

                 //new SysControllerSysAction{
                 //   SysActionId = sysActions.First(a=>a.ActionName=="test1").Id,
                 //   SysControllerId = sysControllers.First(a=>a.ControllerName== "MyMessage").Id},

                 //   new SysControllerSysAction{
                 //   SysActionId = sysActions.First(a=>a.ActionName=="test2").Id,
                 //   SysControllerId = sysControllers.First(a=>a.ControllerName== "MyMessage").Id}

            });

            #endregion

            #region 角色定义
            //自动清理不需要的权限设置功能
            var ids4SysControllerSysActions = sysControllerSysActions.Select(a => a.Id).ToList();
            context.SysRoleSysControllerSysActions.RemoveRange(context.SysRoleSysControllerSysActions.Where(rc => !ids4SysControllerSysActions.Contains(rc.SysControllerSysActionId)).ToList());
            //context.Commit(); 实时提交一下是否能解决重复的问题？
            foreach (var ent in sysEnterprises)
            {
                //生成系统管理员
                var sysRole = new SysRole
                {
                    Id = "admin-" + ent.Id,
                    Name = ent.EnterpriseName + "系统管理员",
                    RoleName = "系统管理员",
                    SysDefault = true,
                    SystemId = "999",
                    EnterpriseId = ent.Id,
                    //系统管理员自动获得所有权限
                    //SysRoleSysControllerSysActions = (from aa in sysControllerSysActions select new SysRoleSysControllerSysAction { SysControllerSysActionId = aa.Id, RoleId = "admin-" + ent.Id }).ToArray()
                };
                context.SysRoles.AddOrUpdate(sysRole);
                //系统管理员自动获得所有权限
                var sysRoleSysControllerSysActions = (from aa in sysControllerSysActions
                                                      select
                                                          new SysRoleSysControllerSysAction()
                                                          {
                                                              SysControllerSysActionId = aa.Id,
                                                              RoleId = sysRole.Id
                                                          }).ToArray();
                context.SysRoleSysControllerSysActions.AddOrUpdate(rc => new { rc.RoleId, rc.SysControllerSysActionId }, sysRoleSysControllerSysActions);
            }
            #endregion

        }
    }
}