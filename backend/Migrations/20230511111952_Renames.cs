using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JournalAPI.Migrations
{
    /// <inheritdoc />
    public partial class Renames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsWithVersion_Groups_GroupId",
                table: "GroupsWithVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_Relations_SubgroupsWithVersion_SubgroupId",
                table: "Relations");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubgroup_SubgroupsWithVersion_SubgroupId",
                table: "StudentSubgroup");

            migrationBuilder.DropForeignKey(
                name: "FK_SubgroupsWithVersion_GroupsWithVersion_GroupWithVersionId",
                table: "SubgroupsWithVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubgroupsWithVersion",
                table: "SubgroupsWithVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "SubgroupsWithVersion",
                newName: "Subgroups");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "MainGroups");

            migrationBuilder.RenameIndex(
                name: "IX_SubgroupsWithVersion_GroupWithVersionId",
                table: "Subgroups",
                newName: "IX_Subgroups_GroupWithVersionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subgroups",
                table: "Subgroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainGroups",
                table: "MainGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsWithVersion_MainGroups_GroupId",
                table: "GroupsWithVersion",
                column: "GroupId",
                principalTable: "MainGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_Subgroups_SubgroupId",
                table: "Relations",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubgroup_Subgroups_SubgroupId",
                table: "StudentSubgroup",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subgroups_GroupsWithVersion_GroupWithVersionId",
                table: "Subgroups",
                column: "GroupWithVersionId",
                principalTable: "GroupsWithVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsWithVersion_MainGroups_GroupId",
                table: "GroupsWithVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_Relations_Subgroups_SubgroupId",
                table: "Relations");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubgroup_Subgroups_SubgroupId",
                table: "StudentSubgroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Subgroups_GroupsWithVersion_GroupWithVersionId",
                table: "Subgroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subgroups",
                table: "Subgroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainGroups",
                table: "MainGroups");

            migrationBuilder.RenameTable(
                name: "Subgroups",
                newName: "SubgroupsWithVersion");

            migrationBuilder.RenameTable(
                name: "MainGroups",
                newName: "Groups");

            migrationBuilder.RenameIndex(
                name: "IX_Subgroups_GroupWithVersionId",
                table: "SubgroupsWithVersion",
                newName: "IX_SubgroupsWithVersion_GroupWithVersionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubgroupsWithVersion",
                table: "SubgroupsWithVersion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsWithVersion_Groups_GroupId",
                table: "GroupsWithVersion",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_SubgroupsWithVersion_SubgroupId",
                table: "Relations",
                column: "SubgroupId",
                principalTable: "SubgroupsWithVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubgroup_SubgroupsWithVersion_SubgroupId",
                table: "StudentSubgroup",
                column: "SubgroupId",
                principalTable: "SubgroupsWithVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubgroupsWithVersion_GroupsWithVersion_GroupWithVersionId",
                table: "SubgroupsWithVersion",
                column: "GroupWithVersionId",
                principalTable: "GroupsWithVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
