Create Procedure spUpdateAppointment
@AppointmentId int,
@Date Date,
@VisitTime time,
@EndTime time
AS
Begin
	Begin Try
		Begin Tran
		Declare @result int=0;
		IF Exists(Select * from AppointmentsTable Where @AppointmentId=@AppointmentId)
			Begin
				Update AppointmentsTable Set AppointmentDate=@Date,VisitTime=@VisitTime,EndTime=@EndTime Where AppointmentId=@AppointmentId;
				set @result=1;
			End
		Else
			begin
				set @result=0;
			end
		Commit Tran
		return @result;
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


select * from AppointmentsTable
select * from DoctorTable