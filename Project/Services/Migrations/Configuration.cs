using Models.SysModels;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Services.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            this.CommandTimeout = 180;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            #region 企业

            var sysEnterprises = new[]
            {
                new SysEnterprise
                {
                    Id = "defaultEnt",
                    EnterpriseName = "系统默认企业"
                },
                new SysEnterprise
                {
                    Id = "TestEnt",
                    EnterpriseName = "测试企业"
                }
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
                    Name = "管理平台",
                    ControllerName = "Index",
                    ActionName="Index",
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

                #endregion other                

                #region 任务管理 800
                  new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "任务管理",
                   SystemId = "800",
                    Ico = "fa-line-chart",
                    Display = true
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "任务中心",
                    ControllerName = "TaskCenter",
                    SystemId = "800200",
                    Ico = "fa-tasks"
                },
                #endregion


                #region 用户管理 900
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "用户管理",
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
                    Ico = "fa-sitemap"
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
                    Ico = "fa-user-o"
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "用户日志",
                    ControllerName = "SysUserLog",
                    SystemId = "900990",
                    Ico = "fa-calendar"
                },
                #endregion

                
                #region 系统开发配置 950
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统设置",
                    SystemId = "950",
                    Ico = "fa-cog",IsSystem = true
                }, new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统参数",
                    ControllerName = "WebConfigAppSetting",
                    SystemId = "950200",
                    Ico = "fa-cubes",IsSystem = true
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "操作类型",
                    ControllerName = "SysAction",
                    SystemId = "950300",
                    Ico = "fa-th-large",IsSystem = true
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统模块",
                    ControllerName = "SysController",
                    SystemId = "950400",
                    Ico = "fa-puzzle-piece",IsSystem = true
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "帮助信息",
                    ControllerName = "SysHelp",
                    SystemId = "950900",
                    Ico = "fa-info-circle",IsSystem = true
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "帮助信息分类",
                    ControllerName = "SysHelpClass",
                    SystemId = "950950",
                    Ico = "fa-info-circle",IsSystem = true
                },
             
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统日志",
                    ControllerName = "SysLog",
                    SystemId = "950995",
                    Ico = "fa-file-text-o",IsSystem = true
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

            context.SysControllerSysActions.AddOrUpdate(a => new { a.SysActionId, a.SysControllerId });

            #endregion

            #region 角色定义

            //自动清理不需要的权限设置功能
            var ids4SysControllerSysActions = sysControllerSysActions.Select(a => a.Id).ToList();
            context.SysRoleSysControllerSysActions.RemoveRange(
                context.SysRoleSysControllerSysActions.Where(
                    rc =>
                        rc.SysControllerSysAction.SysAction.System &&
                        !ids4SysControllerSysActions.Contains(rc.SysControllerSysActionId)).ToList());
            //context.Commit(); 实时提交一下是否能解决重复的问题？
            foreach (var ent in sysEnterprises)
            {
                var sysRoleUser = new SysRole
                {
                    Id = "User-" + ent.Id,
                    Name = ent.EnterpriseName + "注册用户",
                    RoleName = "注册用户",
                    SysDefault = true,
                    SystemId = "000",
                    EnterpriseId = ent.Id
                };
                context.SysRoles.AddOrUpdate(sysRoleUser);

                //生成系统管理员
                var sysRole = new SysRole
                {
                    Id = "admin-" + ent.Id,
                    Name = ent.EnterpriseName + "系统管理员",
                    RoleName = "系统管理员",
                    SysDefault = true,
                    SystemId = "999",
                    EnterpriseId = ent.Id
                    //系统管理员自动获得所有权限
                    //SysRoleSysControllerSysActions = (from aa in sysControllerSysActions select new SysRoleSysControllerSysAction { SysControllerSysActionId = aa.Id, RoleId = "admin-" + ent.Id }).ToArray()
                };
                context.SysRoles.AddOrUpdate(sysRole);
                //系统管理员自动获得所有权限
                var sysRoleSysControllerSysActions = (from aa in sysControllerSysActions
                                                      select
                                                      new SysRoleSysControllerSysAction
                                                      {
                                                          SysControllerSysActionId = aa.Id,
                                                          RoleId = sysRole.Id
                                                      }).ToArray();
                context.SysRoleSysControllerSysActions.AddOrUpdate(rc => new { rc.RoleId, rc.SysControllerSysActionId },
                    sysRoleSysControllerSysActions);
            }

            #endregion
        }
    }
}
