using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database;

public static class ModelBuilderSeedExtensions
{
    public static void SeedInitialData(this ModelBuilder modelBuilder)
    {
        // generate IDs once to keep relationships consistent
        //var adminRoleId = Guid.NewGuid().ToString();
        //var userRoleId = Guid.NewGuid().ToString();
        //var managerRoleId = Guid.NewGuid().ToString();

        //var createPermId = Guid.NewGuid().ToString();
        //var assignPermId = Guid.NewGuid().ToString();
        //var updatePermId = Guid.NewGuid().ToString();
        //var updateRolePermId = Guid.NewGuid().ToString();
        //var deletePermId = Guid.NewGuid().ToString();
        //var readPermId = Guid.NewGuid().ToString();

        //var createResId = Guid.NewGuid().ToString();
        //var assignResId = Guid.NewGuid().ToString();
        //var updateResId = Guid.NewGuid().ToString();
        //var updateResId2 = Guid.NewGuid().ToString();
        //var updateRoleResId = Guid.NewGuid().ToString();
        //var deleteResId = Guid.NewGuid().ToString();
        //var getResId = Guid.NewGuid().ToString();

        // ---- Roles ----
        //modelBuilder.Entity<Role>().HasData(
        //    new Role { Id = adminRoleId, Name = "Admin" },
        //    new Role { Id = userRoleId, Name = "User" },
        //    new Role { Id = managerRoleId, Name = "Manager" }
        //);

        // ---- Permissions ----
        //modelBuilder.Entity<Permission>().HasData(
        //    new Permission { Id = createPermId, Name = "CreateUser" },
        //    new Permission { Id = assignPermId, Name = "AssignRole" },
        //    new Permission { Id = updatePermId, Name = "UpdateUser" },
        //    new Permission { Id = updateRolePermId, Name = "UpdateRole" },
        //    new Permission { Id = deletePermId, Name = "DeleteUser" },
        //    new Permission { Id = readPermId, Name = "Read" }
        //);

        // ---- Resources ----
        //modelBuilder.Entity<Resource>().HasData(
        //    new Resource { Id = createResId, ResourceName = "/api/users/special/create", ResourceType = "api", ResourceMethod = "POST" },
        //    new Resource { Id = assignResId, ResourceName = "/api/users/assign-role", ResourceType = "api", ResourceMethod = "POST" },
        //    new Resource { Id = updateResId, ResourceName = "/api/users/special/update", ResourceType = "api", ResourceMethod = "PUT" },
        //    new Resource { Id = updateResId2, ResourceName = "/api/users/update", ResourceType = "api", ResourceMethod = "PUT" },
        //    new Resource { Id = updateRoleResId, ResourceName = "/api/users/update-role", ResourceType = "api", ResourceMethod = "PUT" },
        //    new Resource { Id = deleteResId, ResourceName = "/api/users/special/delete", ResourceType = "api", ResourceMethod = "DELETE" },
        //    new Resource { Id = getResId, ResourceName = "/api/users/get", ResourceType = "api", ResourceMethod = "GET" }
        //);

        // ---- ResourcePermissions ----
        //modelBuilder.Entity<ResourcePermission>().HasData(
        //    new ResourcePermission { ResourceId = createResId, PermissionId = createPermId },
        //    new ResourcePermission { ResourceId = updateResId, PermissionId = createPermId },
        //    new ResourcePermission { ResourceId = assignResId, PermissionId = assignPermId },
        //    new ResourcePermission { ResourceId = updateRoleResId, PermissionId = assignPermId },
        //    new ResourcePermission { ResourceId = updateResId, PermissionId = updatePermId },
        //    new ResourcePermission { ResourceId = updateRoleResId, PermissionId = updateRolePermId },
        //    new ResourcePermission { ResourceId = deleteResId, PermissionId = deletePermId },
        //    new ResourcePermission { ResourceId = getResId, PermissionId = readPermId },
        //    new ResourcePermission { ResourceId = updateResId2, PermissionId = readPermId }
        //);

        // ---- RolePermissions ----
        //modelBuilder.Entity<RolePermission>().HasData(
        //    // Admin full access
        //    new RolePermission { RoleId = adminRoleId, PermissionId = createPermId },
        //    new RolePermission { RoleId = adminRoleId, PermissionId = assignPermId },
        //    new RolePermission { RoleId = adminRoleId, PermissionId = updatePermId },
        //    new RolePermission { RoleId = adminRoleId, PermissionId = updateRolePermId },
        //    new RolePermission { RoleId = adminRoleId, PermissionId = deletePermId },
        //    new RolePermission { RoleId = adminRoleId, PermissionId = readPermId },

        //    // User read only
        //    new RolePermission { RoleId = userRoleId, PermissionId = readPermId },

        //    // Manager can read and update
        //    new RolePermission { RoleId = managerRoleId, PermissionId = readPermId },
        //    new RolePermission { RoleId = managerRoleId, PermissionId = updatePermId },
        //    new RolePermission { RoleId = managerRoleId, PermissionId = updateRolePermId },
        //    new RolePermission { RoleId = managerRoleId, PermissionId = assignPermId }
        //);
    }
}

