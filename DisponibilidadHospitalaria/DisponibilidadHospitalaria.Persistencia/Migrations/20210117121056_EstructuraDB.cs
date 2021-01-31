using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class EstructuraDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provincias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposDeInstitucion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denominacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDeInstitucion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposDeUnidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denominacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDeUnidad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ciudades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinciaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciudades_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instituciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDeInstitucionId = table.Column<int>(type: "int", nullable: false),
                    CiudadId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Direccion_CallePrincipal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion_CalleSecundaria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion_Numeracion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Direccion_Longitud = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Direccion_Latitud = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instituciones_Ciudades_CiudadId",
                        column: x => x.CiudadId,
                        principalTable: "Ciudades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instituciones_TiposDeInstitucion_TipoDeInstitucionId",
                        column: x => x.TipoDeInstitucionId,
                        principalTable: "TiposDeInstitucion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstitucionesUnidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstitucionId = table.Column<int>(type: "int", nullable: false),
                    TipoDeUnidadId = table.Column<int>(type: "int", nullable: false),
                    Denominacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Capacidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitucionesUnidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitucionesUnidades_Instituciones_InstitucionId",
                        column: x => x.InstitucionId,
                        principalTable: "Instituciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitucionesUnidades_TiposDeUnidad_TipoDeUnidadId",
                        column: x => x.TipoDeUnidadId,
                        principalTable: "TiposDeUnidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosAsignados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstitucionId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosAsignados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosAsignados_Instituciones_InstitucionId",
                        column: x => x.InstitucionId,
                        principalTable: "Instituciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Disponibilidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ocupadas = table.Column<int>(type: "int", nullable: false),
                    Disponibles = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disponibilidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disponibilidades_InstitucionesUnidades_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "InstitucionesUnidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_ProvinciaId_Nombre",
                table: "Ciudades",
                columns: new[] { "ProvinciaId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disponibilidades_UnidadId_Fecha",
                table: "Disponibilidades",
                columns: new[] { "UnidadId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Instituciones_CiudadId",
                table: "Instituciones",
                column: "CiudadId");

            migrationBuilder.CreateIndex(
                name: "IX_Instituciones_Nombre",
                table: "Instituciones",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instituciones_TipoDeInstitucionId",
                table: "Instituciones",
                column: "TipoDeInstitucionId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitucionesUnidades_InstitucionId",
                table: "InstitucionesUnidades",
                column: "InstitucionId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitucionesUnidades_TipoDeUnidadId",
                table: "InstitucionesUnidades",
                column: "TipoDeUnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_Codigo",
                table: "Provincias",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_Nombre",
                table: "Provincias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAsignados_Email",
                table: "UsuariosAsignados",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAsignados_Email_InstitucionId",
                table: "UsuariosAsignados",
                columns: new[] { "Email", "InstitucionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAsignados_InstitucionId",
                table: "UsuariosAsignados",
                column: "InstitucionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disponibilidades");

            migrationBuilder.DropTable(
                name: "UsuariosAsignados");

            migrationBuilder.DropTable(
                name: "InstitucionesUnidades");

            migrationBuilder.DropTable(
                name: "Instituciones");

            migrationBuilder.DropTable(
                name: "TiposDeUnidad");

            migrationBuilder.DropTable(
                name: "Ciudades");

            migrationBuilder.DropTable(
                name: "TiposDeInstitucion");

            migrationBuilder.DropTable(
                name: "Provincias");
        }
    }
}
