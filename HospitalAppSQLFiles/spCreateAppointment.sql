Alter Procedure spCreateAppointment
@PatientId int,
@DoctorId int,
@Number int,
@Date Date,
@Time time,
@EndTime time
AS
Begin
	Begin Try
		declare @result int
		IF Exists(select DoctorId from AppointmentsTable where DoctorId In(select DoctorId from AppointmentsTable Where @Time between VisitTime and EndTime And DoctorId=@DoctorId))
		Begin
			set @result=0;
			select @result;
			print 'Appointment already exist';
			--break;
		end
		Else
		begin
			Begin Tran
				declare @patName varchar(100),@email varchar(100),@injury varchar(100),@DocName varchar(100)
				IF Exists(Select * from PatientTable Where PatientId=@PatientId)
				Begin
					set @patName =(select patientFirstName from PatientTable where PatientId=@PatientId);
					set @email =(select PatientEmail from PatientTable where PatientId=@PatientId);
					set @injury =(select PatientDesies from PatientTable where PatientId=@PatientId);
					--set @patName =(select patientFirstName from PatientTable where PatientId=@PatientId);
					set @DocName =(select DoctorName from DoctorTable where DoctorId=@DoctorId);

					Insert Into AppointmentsTable (PatientId,PatientName,PatientEmail,AppointmentDate,VisitTime,PatientNumber,DoctorId,DoctorName,desies,EndTime) Values(@PatientId,@patName,@email,@Date,@Time,@Number,@DoctorId,@DocName,@injury,@EndTime);
					set @result=1
					select @result;
				End
				Else
				begin
					set @result=2
					select @result;
					print 'Invalid Input';
				end
			Commit Tran
		end
	End Try
	Begin Catch
	-- Transaction uncommittable
		IF (XACT_STATE()) = -1
		ROLLBACK TRANSACTION
 
		-- Transaction committable
		IF (XACT_STATE()) = 1
		COMMIT TRANSACTION
	End Catch
End;


exec spCreateAppointment 1,1,8987,'06-05-2023','10:20','10:50';
select * from AppointmentsTable

