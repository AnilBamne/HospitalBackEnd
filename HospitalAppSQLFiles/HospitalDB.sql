--creating tables--
-- 1 doctor ---
Create table DoctorTable(
DoctorId int primary key Identity(1,1),
DoctorName varchar(250),
DoctorGender varchar(50),
DoctorSpecialization varchar(250),
DoctorNumber int,
DoctorEmail varchar(200),
DoctorPassword varchar(200)

);

-- 2 patient  --
Create table PatientTable(
PatientId int primary key identity(1,1),
patientFirstName varchar(250),
patientLastName varchar(250),
PatientEmail varchar(200),
PatientPassword varchar(200),
PatientGender varchar(50),
PatientAddress varchar(200),
PatientCity varchar(200),
PatientState varchar(200),
PatientDesies varchar(250),
--DoctorId int Foreign key References DoctorTable(DoctorId)
);

Alter Table PatientTable
Add Trash bit Default 0 Not Null


-- 3 Appointment --

create table AppointmentsTable(
AppointmentId int primary key identity(1,1),
PatientId int Foreign key References PatientTable(PatientId),
PatientName varchar(100),
PatientEmail varchar(100),
AppointmentDate DateTime,
VisitTime time,
PatientNumber int,
DoctorId int Foreign key References DoctorTable(DoctorId),
DoctorName varchar(100),
desies varchar(100)
);

--Alter Table AppointmentsTable
--Add Trash time
Alter Table AppointmentsTable
Add Trash bit Default 0 Not Null

-- 4 Admin --
Create table AdminTable(
AdminId int primary Key Identity(1,1),
AdminName varchar(200),
AdminEmail varchar(200),
AdminPassword varchar(200),
PatientId int Foreign key References PatientTable(PatientId),
DoctorId int Foreign key References DoctorTable(DoctorId)
);

select * from DoctorTable
select * from PatientTable
select * from AdminTable
select * from AppointmentsTable

