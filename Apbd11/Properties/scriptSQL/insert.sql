-- Doctors
INSERT INTO Doctor (FirstName, LastName, Email) VALUES ('John', 'Doe', 'john.doe@example.com');

-- Patients
INSERT INTO Patient (FirstName, LastName, Birthdate) VALUES ('Jane', 'Smith', '1985-06-15');

-- Medicaments
INSERT INTO Medicament (Name, Description, Type) VALUES ('Ibuprofen', 'Anti-inflammatory', 'Tablet');

-- Prescriptions
INSERT INTO Prescription (Date, DueDate, IdPatient, IdDoctor) VALUES ('2025-05-20', '2025-05-30', 1, 1);

-- Prescription_Medicaments
INSERT INTO Prescription_Medicament (IdMedicament, IdPrescription, Dose, Details) VALUES (1, 1, 2, 'Take with water');
