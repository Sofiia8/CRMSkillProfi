using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class MainSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonContacts",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "ButtonDesktop",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "ButtonEdit",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "ButtonMain",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "ButtonNetworks",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "ButtonProjects",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "ButtonSave",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "ButtonServices",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MainSettings");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "MainSettings");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "MainSettings",
                newName: "valueField");

            migrationBuilder.RenameColumn(
                name: "Header",
                table: "MainSettings",
                newName: "descriptionField");

            migrationBuilder.AddColumn<int>(
                name: "codeFiled",
                table: "MainSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codeFiled",
                table: "MainSettings");

            migrationBuilder.RenameColumn(
                name: "valueField",
                table: "MainSettings",
                newName: "ImageId");

            migrationBuilder.RenameColumn(
                name: "descriptionField",
                table: "MainSettings",
                newName: "Header");

            migrationBuilder.AddColumn<string>(
                name: "ButtonContacts",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonDesktop",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonEdit",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonMain",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonNetworks",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonProjects",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonSave",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonServices",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MainSettings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
