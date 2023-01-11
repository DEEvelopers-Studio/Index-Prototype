RETURN
INSERT INTO Subjects (id,title,section,attendanceOnStart,defaultStudentSelection) VALUES ('84cd2e3a-f898-4bbf-ade4-921ee59abe37','Math','BS301',1,0);
SELECT * from Subjects where id='84cd2e3a-f898-4bbf-ade4-921ee59abe37';
select Students.*,Attendance.date,ISNULL(Attendance.status, -1) as attendanceStatus from Students LEFT JOIN StudentSubjectData ON Students.uid = StudentSubjectData.uid LEFT JOIN Attendance ON Students.uid=Attendance.studentId AND Attendance.date = '2023-01-10' AND Attendance.subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37'  where StudentSubjectData.subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37'  Order By lastName ASC  

select * from Students
select * from Subjects
select * from Attendance

select Students.* from Students LEFT JOIN StudentSubjectData ON Students.uid = StudentSubjectData.uid where StudentSubjectData.subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37'
SELECT COUNT(status), studentId FROM Attendance WHERE subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37' AND status = 0 GROUP BY studentId;
select Students.*,ISNULL(PRESENT,0) AS PRESENT,ISNULL(LATE,0) AS LATE,ISNULL(ABSENT,0) AS ABSENT   from Students LEFT JOIN StudentSubjectData ON Students.uid = StudentSubjectData.uid Left Join (SELECT COUNT(CASE WHEN status = 0 THEN 1 END) as PRESENT,COUNT(CASE WHEN status = 1 THEN 1 END) as LATE,COUNT(CASE WHEN status = 2 THEN 1 END) as ABSENT, studentId FROM Attendance WHERE subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37' GROUP BY studentId) att on Students.uid = att.studentId

select Subjects.* from Subjects LEFT JOIN TeacherSubjectData ON Subjects.id = TeacherSubjectData.subjectId where TeacherSubjectData.uid = 'e72c9d71-c3d0-42e3-bc48-6e8e30d66608'
select Students.*,totalAttendance.mycount from Students t join (select COUNT(status) as Present from Attendance where subjectId='84cd2e3a-f898-4bbf-ade4-921ee59abe37' AND studentId = Students.uid AND status = 0) tsum on t.uid = Attendance.studentId
select COUNT(status) as Present from Attendance where subjectId='84cd2e3a-f898-4bbf-ade4-921ee59abe37' AND studentId = '02000129455' AND status = 0
select COUNT(status) as Present from studentAttendace
select Teachers.uid, Teachers.firstName,Teachers.lastName,Teachers.middleName,Teachers.lastName from Teachers LEFT JOIN TeacherSubjectData ON Teachers.uid = TeacherSubjectData.uid where TeacherSubjectData.subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37' Order By Teachers.lastName ASC
select date,status from Attendance where studentId = '02000129455' AND subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37'
select * from StudentSubjectData
INSERT into StudentSubjectData (subjectId,uid) values ('84cd2e3a-f898-4bbf-ade4-921ee59abe37','02000129455')

INSERT INTO Students (uid,firstName,middleName,lastName,section) VALUES ('02000129454','Jermaine','C','Marabe','BSCS 301')


DROP TABLE TeacherSubjectData
(SELECT * FROM Attendance WHERE studentId = '02000129453' AND date = CAST(GETDATE() as DATE) AND subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37')

CREATE TABLE [dbo].[TeacherSubjectData] (
    [uid]       VARCHAR (36) NOT NULL,
    [subjectId] VARCHAR (36) NOT NULL,
    FOREIGN KEY ([uid]) REFERENCES [dbo].[Teachers] ([uid]),
    FOREIGN KEY ([subjectId]) REFERENCES [dbo].[Subjects] ([id])
);

