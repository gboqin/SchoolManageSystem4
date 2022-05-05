using Microsoft.EntityFrameworkCore;
using SchoolManageSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Repositorys.Database
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>().HasOne(c => c.TenantInfo)
                .WithMany(c => c.SysUsers)
                .HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<SysUser>().HasIndex(c => new { c.TenantId, c.Id });
            modelBuilder.Entity<SysUser>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<SysRole>().HasIndex(c => new { c.TenantId, c.Id });

            modelBuilder.Entity<SysUserRoleRelation>()
                .HasKey(t => t.Id);
            //.HasKey(t => new { t.RoleId, t.UserId, t.TenantId });
            modelBuilder.Entity<SysUserRoleRelation>()
                .HasOne(ur => ur.SysUser)
                .WithMany(r => r.SysUserRoleRelations)
                .HasForeignKey(c => c.UserId);
            modelBuilder.Entity<SysUserRoleRelation>()
                .HasOne(ur => ur.SysRole)
                .WithMany(r => r.SysUserRoleRelations)
                .HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<SysRoleMenuRelation>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<SysRoleMenuRelation>()
                .HasOne(rm => rm.SysRole)
                .WithMany(r => r.SysRoleMenuRelations)
                 .HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<SysRoleMenuRelation>()
                .HasOne(rm => rm.SysMenu)
                .WithMany(r => r.SysRoleMenuRelations)
                .HasForeignKey(c => c.MenuId);

            new Configuration().AutoRegisterModel(modelBuilder);
        }
    }
}
