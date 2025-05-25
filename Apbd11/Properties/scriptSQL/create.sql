CREATE TABLE Doctor (
    IdDoctor INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(100)
);

CREATE TABLE Patient (
    IdPatient INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Birthdate DATE
);

CREATE TABLE Medicament (
    IdMedicament INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Description NVARCHAR(255),
    Type NVARCHAR(100)
);

CREATE TABLE Prescription (
    IdPrescription INT PRIMARY KEY IDENTITY,
    Date DATE NOT NULL,
    DueDate DATE NOT NULL,
    IdPatient INT FOREIGN KEY REFERENCES Patient(IdPatient),
    IdDoctor INT FOREIGN KEY REFERENCES Doctor(IdDoctor)
);

CREATE TABLE Prescription_Medicament (
    IdMedicament INT,
    IdPrescription INT,
    Dose INT,
    Details NVARCHAR(255),
    PRIMARY KEY (IdMedicament, IdPrescription),
    FOREIGN KEY (IdMedicament) REFERENCES Medicament(IdMedicament),
    FOREIGN KEY (IdPrescription) REFERENCES Prescription(IdPrescription)
);
