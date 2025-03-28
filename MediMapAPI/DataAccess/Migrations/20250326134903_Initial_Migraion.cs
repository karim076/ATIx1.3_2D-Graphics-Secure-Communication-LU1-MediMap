using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migraion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MediMap");

            migrationBuilder.CreateTable(
                name: "Arts",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Specialisatie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OuderVoogd",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoorNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AchterNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OuderVoogd", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traject",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZorgMomnet",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Plaatje = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TijdsduurInMin = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZorgMomnet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "MediMap",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoorNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AvatarNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AchterNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PathLocation = table.Column<int>(type: "int", nullable: false),
                    OuderVoogdId = table.Column<int>(type: "int", nullable: false),
                    TrajectId = table.Column<int>(type: "int", nullable: false),
                    ArtsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Arts_ArtsId",
                        column: x => x.ArtsId,
                        principalSchema: "MediMap",
                        principalTable: "Arts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Patient_OuderVoogd_OuderVoogdId",
                        column: x => x.OuderVoogdId,
                        principalSchema: "MediMap",
                        principalTable: "OuderVoogd",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Traject_TrajectId",
                        column: x => x.TrajectId,
                        principalSchema: "MediMap",
                        principalTable: "Traject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrajectZorgMoment",
                schema: "MediMap",
                columns: table => new
                {
                    TrajectID = table.Column<int>(type: "int", nullable: false),
                    ZorgMomentID = table.Column<int>(type: "int", nullable: false),
                    Volgorde = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrajectZorgMoment", x => new { x.TrajectID, x.ZorgMomentID });
                    table.ForeignKey(
                        name: "FK_TrajectZorgMoment_Traject_TrajectID",
                        column: x => x.TrajectID,
                        principalSchema: "MediMap",
                        principalTable: "Traject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrajectZorgMoment_ZorgMomnet_ZorgMomentID",
                        column: x => x.ZorgMomentID,
                        principalSchema: "MediMap",
                        principalTable: "ZorgMomnet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogBooks",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Log = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogBooks_Patient_PatientID",
                        column: x => x.PatientID,
                        principalSchema: "MediMap",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileInformation",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NaamDokter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BehandelPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AfspraakDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ArtsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileInformation_Arts_ArtsId",
                        column: x => x.ArtsId,
                        principalSchema: "MediMap",
                        principalTable: "Arts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileInformation_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "MediMap",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PatienId = table.Column<int>(type: "int", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Patient_PatienId",
                        column: x => x.PatienId,
                        principalSchema: "MediMap",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "MediMap",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "MediMap",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "MediMap",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "MediMap",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "MediMap",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "MediMap",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "MediMap",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "MediMap",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogBooks_PatientID",
                schema: "MediMap",
                table: "LogBooks",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_ArtsId",
                schema: "MediMap",
                table: "Patient",
                column: "ArtsId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_OuderVoogdId",
                schema: "MediMap",
                table: "Patient",
                column: "OuderVoogdId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_TrajectId",
                schema: "MediMap",
                table: "Patient",
                column: "TrajectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInformation_ArtsId",
                schema: "MediMap",
                table: "ProfileInformation",
                column: "ArtsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInformation_PatientId",
                schema: "MediMap",
                table: "ProfileInformation",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "MediMap",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "MediMap",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrajectZorgMoment_ZorgMomentID",
                schema: "MediMap",
                table: "TrajectZorgMoment",
                column: "ZorgMomentID");

            migrationBuilder.CreateIndex(
                name: "IX_User_PatienId",
                schema: "MediMap",
                table: "User",
                column: "PatienId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "MediMap",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "MediMap",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "MediMap",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "MediMap",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogBooks",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "ProfileInformation",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "TrajectZorgMoment",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "ZorgMomnet",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "User",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "Patient",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "Arts",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "OuderVoogd",
                schema: "MediMap");

            migrationBuilder.DropTable(
                name: "Traject",
                schema: "MediMap");
        }
    }
}
