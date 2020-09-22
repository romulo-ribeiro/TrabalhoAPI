using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Context.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    IdCourse = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<int>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.IdCourse);
                });

            migrationBuilder.CreateTable(
                name: "Materia",
                columns: table => new
                {
                    IdSubject = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Registry = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materia", x => x.IdSubject);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUser = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Surname = table.Column<string>(maxLength: 100, nullable: true),
                    Cpf = table.Column<string>(maxLength: 11, nullable: true),
                    Role = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "Contem",
                columns: table => new
                {
                    IdSubject = table.Column<int>(nullable: false),
                    IdCourse = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contem", x => x.IdSubject);
                    table.ForeignKey(
                        name: "FK_Contem_Curso_IdCourse",
                        column: x => x.IdCourse,
                        principalTable: "Curso",
                        principalColumn: "IdCourse",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contem_Materia_IdSubject",
                        column: x => x.IdSubject,
                        principalTable: "Materia",
                        principalColumn: "IdSubject",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lotacao",
                columns: table => new
                {
                    IdCourse = table.Column<int>(nullable: false),
                    IdUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotacao", x => x.IdCourse);
                    table.ForeignKey(
                        name: "FK_Lotacao_Curso_IdCourse",
                        column: x => x.IdCourse,
                        principalTable: "Curso",
                        principalColumn: "IdCourse",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotacao_Usuario_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Usuario",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Matricula",
                columns: table => new
                {
                    IdSubject = table.Column<int>(nullable: false),
                    IdUser = table.Column<string>(nullable: true),
                    Grade = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matricula", x => x.IdSubject);
                    table.ForeignKey(
                        name: "FK_Matricula_Materia_IdSubject",
                        column: x => x.IdSubject,
                        principalTable: "Materia",
                        principalColumn: "IdSubject",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matricula_Usuario_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Usuario",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contem_IdCourse",
                table: "Contem",
                column: "IdCourse");

            migrationBuilder.CreateIndex(
                name: "IX_Lotacao_IdUser",
                table: "Lotacao",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Matricula_IdUser",
                table: "Matricula",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contem");

            migrationBuilder.DropTable(
                name: "Lotacao");

            migrationBuilder.DropTable(
                name: "Matricula");

            migrationBuilder.DropTable(
                name: "Curso");

            migrationBuilder.DropTable(
                name: "Materia");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
