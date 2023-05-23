Create Procedure spPatResetPassword
@Email varchar(200),
@ConfirmPassword varchar(200)
AS
Begin
	Begin Try
		Begin Transaction
			If Exists(Select * from PatientTable where PatientEmail=@Email)
				Begin
					Update PatientTable Set PatientPassword=@ConfirmPassword where PatientEmail=@Email;
				End
			Else
				Begin
					print 'Email not found';
				End
		Commit Transaction
	End Try
	Begin Catch
		Rollback Transaction
	End Catch
End;

select * from PatientTable