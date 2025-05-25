using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Apbd11.Properties.controllers;
using Apbd11.Properties.models;
using Apbd11.Properties.services;



namespace Apbd11.Properties.tests
{
    public class PrescriptionsControllerTests
    {
        private readonly Mock<IPrescriptionService> _serviceMock;
        private readonly PrescriptionsController _controller;

        public PrescriptionsControllerTests()
        {
            _serviceMock = new Mock<IPrescriptionService>();
            _controller = new PrescriptionsController(_serviceMock.Object);
        }

        [Fact]
        public async Task AddPrescription_WhenValid_ReturnsOk()
        {
            var dto = new PrescriptionRequestDto
            {
                IdDoctor = 1,
                Patient = new PatientDto
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Birthdate = new DateTime(1990, 1, 1)
                },
                Medicaments = new List<MedicamentDto>
                {
                    new MedicamentDto { IdMedicament = 1, Dose = 2, Details = "Take after meal" }
                },
                Date = DateTime.Today,
                DueDate = DateTime.Today.AddDays(5)
            };

            var result = await _controller.AddPrescription(dto);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddPrescription_WhenDueDateInvalid_ReturnsBadRequest()
        {
            var dto = new PrescriptionRequestDto
            {
                IdDoctor = 1,
                Patient = new PatientDto
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Birthdate = new DateTime(1985, 5, 5)
                },
                Medicaments = new List<MedicamentDto>
                {
                    new MedicamentDto { IdMedicament = 2, Dose = 1, Details = "Night" }
                },
                Date = DateTime.Today,
                DueDate = DateTime.Today.AddDays(-1)
            };

            _serviceMock.Setup(x => x.AddPrescriptionAsync(It.IsAny<PrescriptionRequestDto>()))
                .ThrowsAsync(new ArgumentException("Invalid dates"));

            var result = await _controller.AddPrescription(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid dates", badRequest.Value);
        }

        [Fact]
        public async Task GetPatientDetails_WhenExists_ReturnsPatient()
        {
            var patient = new PatientDetailsDto
            {
                IdPatient = 1,
                FirstName = "Anna"
            };

            _serviceMock.Setup(x => x.GetPatientDetailsAsync(1))
                .ReturnsAsync(patient);

            var result = await _controller.GetPatientDetails(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var returnedPatient = Assert.IsType<PatientDetailsDto>(ok.Value);
            Assert.Equal(1, returnedPatient.IdPatient);
        }

        [Fact]
        public async Task GetPatientDetails_WhenNotFound_ReturnsNotFound()
        {
            _serviceMock.Setup(x => x.GetPatientDetailsAsync(123))
                .ReturnsAsync((PatientDetailsDto?)null);
            var result = await _controller.GetPatientDetails(123);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
