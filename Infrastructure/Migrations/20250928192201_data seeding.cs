using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dataseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "069f8c1f-7957-49c8-8ef0-42f2aa318343", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(2310), false, "DeleteUser", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(2311) },
                    { "4d3f2168-56f0-432a-aabb-4ca7316af375", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(1767), false, "CreateUser", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(1772) },
                    { "57d05878-10f8-4c77-a064-8c24159a1a74", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(2319), false, "Read", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(2320) },
                    { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(2176), false, "AssignRole", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(2176) },
                    { "c1e6e530-0d73-4ee3-869b-5facc7d86953", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(2291), false, "UpdateUser", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(2292) },
                    { "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(2301), false, "UpdateRole", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(2302) }
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "ParentResourceId", "ResourceMethod", "ResourceName", "ResourceType", "UpdatedAt" },
                values: new object[,]
                {
                    { "0380cc06-6937-40e2-b8bf-7d9a17eb127b", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(6192), false, null, "PUT", "/api/users/update", "api", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(6193) },
                    { "806ae3a9-6d4b-44e2-beb3-5a3d0c33db27", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(6156), false, null, "POST", "/api/users/assign-role", "api", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(6159) },
                    { "933334fe-2277-448d-afd5-aacda49094e7", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(4627), false, null, "POST", "/api/users/special/create", "api", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(4628) },
                    { "a2b89243-884a-4adb-92f1-102462bec90f", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(6201), false, null, "PUT", "/api/users/update-role", "api", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(6202) },
                    { "aa7bce7c-8d47-4f6d-bf24-4a22bfe15275", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(6209), false, null, "DELETE", "/api/users/special/delete", "api", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(6209) },
                    { "e0341ccb-a538-4105-9256-4481e649c139", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(6181), false, null, "PUT", "/api/users/special/update", "api", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(6182) },
                    { "fdd1b674-7fee-415f-a945-d3469ed4f350", new DateTime(2025, 9, 28, 19, 22, 0, 738, DateTimeKind.Utc).AddTicks(6218), false, null, "GET", "/api/users/get", "api", new DateTime(2025, 9, 29, 0, 52, 0, 738, DateTimeKind.Local).AddTicks(6219) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "ae4094ef-d37d-40d8-b963-d267daaaf997", new DateTime(2025, 9, 28, 19, 22, 0, 733, DateTimeKind.Utc).AddTicks(9850), false, "Admin", new DateTime(2025, 9, 29, 0, 52, 0, 733, DateTimeKind.Local).AddTicks(9948) },
                    { "d5b4010a-1235-45f2-893a-ac49f1a8bb43", new DateTime(2025, 9, 28, 19, 22, 0, 737, DateTimeKind.Utc).AddTicks(1758), false, "User", new DateTime(2025, 9, 29, 0, 52, 0, 737, DateTimeKind.Local).AddTicks(1763) },
                    { "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8", new DateTime(2025, 9, 28, 19, 22, 0, 737, DateTimeKind.Utc).AddTicks(1797), false, "Manager", new DateTime(2025, 9, 29, 0, 52, 0, 737, DateTimeKind.Local).AddTicks(1797) }
                });

            migrationBuilder.InsertData(
                table: "ResourcePermissions",
                columns: new[] { "PermissionId", "ResourceId" },
                values: new object[,]
                {
                    { "57d05878-10f8-4c77-a064-8c24159a1a74", "0380cc06-6937-40e2-b8bf-7d9a17eb127b" },
                    { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "806ae3a9-6d4b-44e2-beb3-5a3d0c33db27" },
                    { "4d3f2168-56f0-432a-aabb-4ca7316af375", "933334fe-2277-448d-afd5-aacda49094e7" },
                    { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "a2b89243-884a-4adb-92f1-102462bec90f" },
                    { "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f", "a2b89243-884a-4adb-92f1-102462bec90f" },
                    { "069f8c1f-7957-49c8-8ef0-42f2aa318343", "aa7bce7c-8d47-4f6d-bf24-4a22bfe15275" },
                    { "4d3f2168-56f0-432a-aabb-4ca7316af375", "e0341ccb-a538-4105-9256-4481e649c139" },
                    { "c1e6e530-0d73-4ee3-869b-5facc7d86953", "e0341ccb-a538-4105-9256-4481e649c139" },
                    { "57d05878-10f8-4c77-a064-8c24159a1a74", "fdd1b674-7fee-415f-a945-d3469ed4f350" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { "069f8c1f-7957-49c8-8ef0-42f2aa318343", "ae4094ef-d37d-40d8-b963-d267daaaf997" },
                    { "4d3f2168-56f0-432a-aabb-4ca7316af375", "ae4094ef-d37d-40d8-b963-d267daaaf997" },
                    { "57d05878-10f8-4c77-a064-8c24159a1a74", "ae4094ef-d37d-40d8-b963-d267daaaf997" },
                    { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "ae4094ef-d37d-40d8-b963-d267daaaf997" },
                    { "c1e6e530-0d73-4ee3-869b-5facc7d86953", "ae4094ef-d37d-40d8-b963-d267daaaf997" },
                    { "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f", "ae4094ef-d37d-40d8-b963-d267daaaf997" },
                    { "57d05878-10f8-4c77-a064-8c24159a1a74", "d5b4010a-1235-45f2-893a-ac49f1a8bb43" },
                    { "57d05878-10f8-4c77-a064-8c24159a1a74", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" },
                    { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" },
                    { "c1e6e530-0d73-4ee3-869b-5facc7d86953", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" },
                    { "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "57d05878-10f8-4c77-a064-8c24159a1a74", "0380cc06-6937-40e2-b8bf-7d9a17eb127b" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "806ae3a9-6d4b-44e2-beb3-5a3d0c33db27" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "4d3f2168-56f0-432a-aabb-4ca7316af375", "933334fe-2277-448d-afd5-aacda49094e7" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "a2b89243-884a-4adb-92f1-102462bec90f" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f", "a2b89243-884a-4adb-92f1-102462bec90f" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "069f8c1f-7957-49c8-8ef0-42f2aa318343", "aa7bce7c-8d47-4f6d-bf24-4a22bfe15275" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "4d3f2168-56f0-432a-aabb-4ca7316af375", "e0341ccb-a538-4105-9256-4481e649c139" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "c1e6e530-0d73-4ee3-869b-5facc7d86953", "e0341ccb-a538-4105-9256-4481e649c139" });

            migrationBuilder.DeleteData(
                table: "ResourcePermissions",
                keyColumns: new[] { "PermissionId", "ResourceId" },
                keyValues: new object[] { "57d05878-10f8-4c77-a064-8c24159a1a74", "fdd1b674-7fee-415f-a945-d3469ed4f350" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "069f8c1f-7957-49c8-8ef0-42f2aa318343", "ae4094ef-d37d-40d8-b963-d267daaaf997" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "4d3f2168-56f0-432a-aabb-4ca7316af375", "ae4094ef-d37d-40d8-b963-d267daaaf997" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "57d05878-10f8-4c77-a064-8c24159a1a74", "ae4094ef-d37d-40d8-b963-d267daaaf997" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "ae4094ef-d37d-40d8-b963-d267daaaf997" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "c1e6e530-0d73-4ee3-869b-5facc7d86953", "ae4094ef-d37d-40d8-b963-d267daaaf997" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f", "ae4094ef-d37d-40d8-b963-d267daaaf997" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "57d05878-10f8-4c77-a064-8c24159a1a74", "d5b4010a-1235-45f2-893a-ac49f1a8bb43" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "57d05878-10f8-4c77-a064-8c24159a1a74", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "c1e6e530-0d73-4ee3-869b-5facc7d86953", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f", "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8" });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "069f8c1f-7957-49c8-8ef0-42f2aa318343");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "4d3f2168-56f0-432a-aabb-4ca7316af375");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "57d05878-10f8-4c77-a064-8c24159a1a74");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "ae9e8307-38b9-40fa-9f61-4ee258ecd8e5");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "c1e6e530-0d73-4ee3-869b-5facc7d86953");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "ea9455a0-73e4-4d6c-8f4a-76ea306efa9f");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "Id",
                keyValue: "0380cc06-6937-40e2-b8bf-7d9a17eb127b");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "Id",
                keyValue: "806ae3a9-6d4b-44e2-beb3-5a3d0c33db27");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "Id",
                keyValue: "933334fe-2277-448d-afd5-aacda49094e7");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "Id",
                keyValue: "a2b89243-884a-4adb-92f1-102462bec90f");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "Id",
                keyValue: "aa7bce7c-8d47-4f6d-bf24-4a22bfe15275");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "Id",
                keyValue: "e0341ccb-a538-4105-9256-4481e649c139");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "Id",
                keyValue: "fdd1b674-7fee-415f-a945-d3469ed4f350");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "ae4094ef-d37d-40d8-b963-d267daaaf997");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d5b4010a-1235-45f2-893a-ac49f1a8bb43");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f83eb35d-fea7-4ce3-b771-b0abbbfce1d8");
        }
    }
}
