using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class AjustesEnUsuarioAsignado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosAsignados_Instituciones_InstitucionId",
                table: "UsuariosAsignados");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosAsignados_Email_InstitucionId",
                table: "UsuariosAsignados");

            migrationBuilder.AlterColumn<int>(
                name: "InstitucionId",
                table: "UsuariosAsignados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "UsuariosAsignados",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsAdministrador",
                table: "UsuariosAsignados",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "UsuariosAsignados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAsignados_Email_InstitucionId",
                table: "UsuariosAsignados",
                columns: new[] { "Email", "InstitucionId" },
                unique: true,
                filter: "[InstitucionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosAsignados_Instituciones_InstitucionId",
                table: "UsuariosAsignados",
                column: "InstitucionId",
                principalTable: "Instituciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosAsignados_Instituciones_InstitucionId",
                table: "UsuariosAsignados");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosAsignados_Email_InstitucionId",
                table: "UsuariosAsignados");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "UsuariosAsignados");

            migrationBuilder.DropColumn(
                name: "EsAdministrador",
                table: "UsuariosAsignados");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "UsuariosAsignados");

            migrationBuilder.AlterColumn<int>(
                name: "InstitucionId",
                table: "UsuariosAsignados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAsignados_Email_InstitucionId",
                table: "UsuariosAsignados",
                columns: new[] { "Email", "InstitucionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosAsignados_Instituciones_InstitucionId",
                table: "UsuariosAsignados",
                column: "InstitucionId",
                principalTable: "Instituciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
