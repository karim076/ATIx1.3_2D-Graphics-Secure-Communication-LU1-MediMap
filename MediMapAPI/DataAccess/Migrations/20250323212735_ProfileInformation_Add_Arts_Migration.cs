using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProfileInformation_Add_Arts_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avatars",
                schema: "MediMap");

            migrationBuilder.AddColumn<int>(
                name: "ArtsId",
                schema: "MediMap",
                table: "ProfileInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AvatarNaam",
                schema: "MediMap",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInformation_ArtsId",
                schema: "MediMap",
                table: "ProfileInformation",
                column: "ArtsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInformation_Arts_ArtsId",
                schema: "MediMap",
                table: "ProfileInformation",
                column: "ArtsId",
                principalSchema: "MediMap",
                principalTable: "Arts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInformation_Arts_ArtsId",
                schema: "MediMap",
                table: "ProfileInformation");

            migrationBuilder.DropIndex(
                name: "IX_ProfileInformation_ArtsId",
                schema: "MediMap",
                table: "ProfileInformation");

            migrationBuilder.DropColumn(
                name: "ArtsId",
                schema: "MediMap",
                table: "ProfileInformation");

            migrationBuilder.DropColumn(
                name: "AvatarNaam",
                schema: "MediMap",
                table: "Patient");

            migrationBuilder.CreateTable(
                name: "Avatars",
                schema: "MediMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    AvatarName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatars_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "MediMap",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_PatientId",
                schema: "MediMap",
                table: "Avatars",
                column: "PatientId");
        }
    }
}
