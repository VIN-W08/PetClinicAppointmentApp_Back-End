using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetClinicAppointmentApp.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    Customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", nullable: false),
                    Password = table.Column<string>(type: "varchar(72)", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.Customer_id);
                });

            migrationBuilder.CreateTable(
                name: "pet_clinic",
                columns: table => new
                {
                    Pet_clinic_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone_number = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Image_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Village_id = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", nullable: false),
                    Password = table.Column<string>(type: "varchar(72)", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_clinic", x => x.Pet_clinic_id);
                });

            migrationBuilder.CreateTable(
                name: "schedule_pet_clinic",
                columns: table => new
                {
                    Schedule_pet_clinic_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pet_clinic_id = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<byte>(type: "tinyint", nullable: false),
                    Shift = table.Column<byte>(type: "tinyint", nullable: false),
                    Start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    End_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_pet_clinic", x => x.Schedule_pet_clinic_id);
                    table.ForeignKey(
                        name: "FK_schedule_pet_clinic_pet_clinic_Pet_clinic_id",
                        column: x => x.Pet_clinic_id,
                        principalTable: "pet_clinic",
                        principalColumn: "Pet_clinic_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    Service_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pet_clinic_id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(19,5)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.Service_id);
                    table.ForeignKey(
                        name: "FK_service_pet_clinic_Pet_clinic_id",
                        column: x => x.Pet_clinic_id,
                        principalTable: "pet_clinic",
                        principalColumn: "Pet_clinic_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedule_service",
                columns: table => new
                {
                    Schedule_service_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Service_id = table.Column<int>(type: "int", nullable: false),
                    Start_schedule = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_schedule = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: 1),
                    Quota = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_service", x => x.Schedule_service_id);
                    table.ForeignKey(
                        name: "FK_schedule_service_service_Service_id",
                        column: x => x.Service_id,
                        principalTable: "service",
                        principalColumn: "Service_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointment",
                columns: table => new
                {
                    Appointment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_id = table.Column<int>(type: "int", nullable: false),
                    Pet_clinic_id = table.Column<int>(type: "int", nullable: false),
                    Service_id = table.Column<int>(type: "int", nullable: true),
                    Schedule_service_id = table.Column<int>(type: "int", nullable: true),
                    Service_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Service_price = table.Column<decimal>(type: "decimal(19,5)", nullable: false),
                    Start_schedule = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_schedule = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment", x => x.Appointment_id);
                    table.ForeignKey(
                        name: "FK_appointment_customer_Customer_id",
                        column: x => x.Customer_id,
                        principalTable: "customer",
                        principalColumn: "Customer_id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_appointment_pet_clinic_Pet_clinic_id",
                        column: x => x.Pet_clinic_id,
                        principalTable: "pet_clinic",
                        principalColumn: "Pet_clinic_id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_appointment_schedule_service_Schedule_service_id",
                        column: x => x.Schedule_service_id,
                        principalTable: "schedule_service",
                        principalColumn: "Schedule_service_id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_appointment_service_Service_id",
                        column: x => x.Service_id,
                        principalTable: "service",
                        principalColumn: "Service_id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointment_Customer_id",
                table: "appointment",
                column: "Customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_Pet_clinic_id",
                table: "appointment",
                column: "Pet_clinic_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_Schedule_service_id",
                table: "appointment",
                column: "Schedule_service_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_Service_id",
                table: "appointment",
                column: "Service_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_Email",
                table: "customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pet_clinic_Email",
                table: "pet_clinic",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_schedule_pet_clinic_Pet_clinic_id",
                table: "schedule_pet_clinic",
                column: "Pet_clinic_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_service_Service_id",
                table: "schedule_service",
                column: "Service_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_Pet_clinic_id",
                table: "service",
                column: "Pet_clinic_id");

            migrationBuilder.Sql("ALTER TABLE pet_clinic ADD CONSTRAINT df_pet_clinic_Created_at DEFAULT sysdatetime() FOR Created_at");
            migrationBuilder.Sql("ALTER TABLE schedule_pet_clinic ADD CONSTRAINT df_schedule_pet_clinic_Created_at DEFAULT sysdatetime() FOR Created_at");
            migrationBuilder.Sql("ALTER TABLE service ADD CONSTRAINT df_service_Created_at DEFAULT sysdatetime() FOR Created_at");
            migrationBuilder.Sql("ALTER TABLE schedule_service ADD CONSTRAINT df_schedule_service_Created_at DEFAULT sysdatetime() FOR Created_at");
            migrationBuilder.Sql("ALTER TABLE appointment ADD CONSTRAINT df_appointment_Created_at DEFAULT sysdatetime() FOR Created_at");
            migrationBuilder.Sql("ALTER TABLE customer ADD CONSTRAINT df_customer_Created_at DEFAULT sysdatetime() FOR Created_at");


            migrationBuilder.Sql("IF EXISTS (SELECT * from sys.triggers WHERE Name = 'tr_update_customer') BEGIN DROP TRIGGER tr_update_customer END");
            migrationBuilder.Sql("CREATE TRIGGER tr_update_customer ON customer FOR UPDATE AS BEGIN UPDATE customer SET Updated_at = SYSDATETIME() FROM Inserted i  WHERE customer.Customer_id = i.Customer_id END");

            migrationBuilder.Sql("IF EXISTS (SELECT * from sys.triggers WHERE Name = 'tr_update_pet_clinic') BEGIN DROP TRIGGER tr_update_pet_clinic END");
            migrationBuilder.Sql("CREATE TRIGGER tr_update_pet_clinic ON pet_clinic FOR UPDATE AS BEGIN UPDATE pet_clinic SET Updated_at = SYSDATETIME() FROM Inserted i  WHERE pet_clinic.Pet_clinic_id = i.Pet_clinic_id END");

            migrationBuilder.Sql("IF EXISTS (SELECT * from sys.triggers WHERE Name = 'tr_update_appointment') BEGIN DROP TRIGGER tr_update_appointment END");
            migrationBuilder.Sql("CREATE TRIGGER tr_update_appointment ON appointment FOR UPDATE AS BEGIN UPDATE appointment SET Updated_at = SYSDATETIME() FROM Inserted i WHERE appointment.Appointment_id = i.Appointment_id END");

            migrationBuilder.Sql("IF EXISTS (SELECT * from sys.triggers WHERE Name = 'tr_update_schedule_pet_clinic') BEGIN DROP TRIGGER tr_update_schedule_pet_clinic END");
            migrationBuilder.Sql("CREATE TRIGGER tr_update_schedule_pet_clinic ON schedule_pet_clinic FOR UPDATE AS BEGIN UPDATE schedule_pet_clinic SET Updated_at = SYSDATETIME() FROM Inserted i WHERE schedule_pet_clinic.Schedule_pet_clinic_id = i.Schedule_pet_clinic_id END");

            migrationBuilder.Sql("IF EXISTS (SELECT * from sys.triggers WHERE Name = 'tr_update_schedule_service') BEGIN DROP TRIGGER tr_update_schedule_service END");
            migrationBuilder.Sql("CREATE TRIGGER tr_update_schedule_service ON schedule_service FOR UPDATE AS BEGIN UPDATE schedule_service SET Updated_at = SYSDATETIME() FROM Inserted i WHERE schedule_service.schedule_service_id = i.schedule_service_id END");

            migrationBuilder.Sql("IF EXISTS (SELECT * from sys.triggers WHERE Name = 'tr_update_service') BEGIN DROP TRIGGER tr_update_service END");
            migrationBuilder.Sql("CREATE TRIGGER tr_update_service ON service FOR UPDATE AS BEGIN UPDATE service SET Updated_at = SYSDATETIME() FROM Inserted i WHERE service.Service_id = i.Service_id END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER tr_update_customer");
            migrationBuilder.Sql("DROP TRIGGER tr_update_pet_clinic");
            migrationBuilder.Sql("DROP TRIGGER tr_update_appointment");
            migrationBuilder.Sql("DROP TRIGGER tr_update_schedule_pet_clinic");
            migrationBuilder.Sql("DROP TRIGGER tr_update_schedule_service");
            migrationBuilder.Sql("DROP TRIGGER tr_update_service");

            migrationBuilder.DropTable(
                name: "appointment");

            migrationBuilder.DropTable(
                name: "schedule_pet_clinic");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "schedule_service");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "pet_clinic");
        }
    }
}
