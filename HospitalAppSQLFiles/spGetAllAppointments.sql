Create Procedure spGetAllAppointments
As
Begin
	Begin Try
		Begin Transaction
			Select * from AppointmentsTable;
		Commit Transaction
	END TRY
	BEGIN CATCH
		-- Transaction uncommittable
		IF (XACT_STATE()) = -1
		ROLLBACK TRANSACTION
 
		-- Transaction committable
		IF (XACT_STATE()) = 1
		COMMIT TRANSACTION
	END CATCH
END;
exec spGetAllAppointments;
select * from PatientTable
select * from DoctorTable

delete from AppointmentsTable where AppointmentId<14