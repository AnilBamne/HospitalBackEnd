Alter Procedure spGetMyPatients
@docId int
As
Begin
	Begin Try
		Begin Transaction
			select 
				p.patientFirstName,
				p.patientLastName,
				p.PatientEmail,
				p.PatientPassword,
				p.PatientGender,
				p.PatientAddress,
				p.PatientCity,
				p.PatientState,
				p.PatientDesies
			from PatientTable p
			Inner Join AppointmentsTable a
			On p.PatientId=a.PatientId
			where DoctorId=@docId
		Commit Transaction
	End Try
	Begin Catch
		rollback transaction
	End Catch
End


exec spGetMyPatients 1
