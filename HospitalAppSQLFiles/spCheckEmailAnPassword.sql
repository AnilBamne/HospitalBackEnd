
-- checking who is logging in
Alter Proc spCheckEmailAnPassword
@email varchar(100),
@password varchar(100)
--@role int output
As
Begin
	Begin try
		Begin Transaction
			declare @role int
			if exists(Select * from DoctorTable where DoctorEmail=@email And DoctorPassword=@password)
				begin
					print 'Its a doctor';
					Set @role=1;
					select @role
				end
				Else
					if exists(Select * from PatientTable where PatientEmail=@email and PatientPassword=@password)
					begin
						Set @role=2;
						select @role
						print 'its a patient'
					end
				Else
					if exists(Select * from AdminTable where AdminEmail=@email And AdminPassword=@password)
					begin
						print 'its admin'
						Set @role=3;
						select @role
					end
				else
					begin
					print 'Invalid input'
					end
		Commit Transaction
	End Try
	Begin Catch
		RollBack Transaction
	End Catch
End;

select * from PatientTable
select * from DoctorTable
select * from AppointmentsTable
delete from PatientTable where PatientId=15