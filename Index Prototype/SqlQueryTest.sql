INSERT INTO Subjects (id,title,section,attendanceOnStart,defaultStudentSelection) VALUES ('84cd2e3a-f898-4bbf-ade4-921ee59abe37','Math','BS301',1,0);
SELECT * from Subjects where id='84cd2e3a-f898-4bbf-ade4-921ee59abe37';
select Students.* from Students LEFT JOIN StudentSubjectData ON Students.uid = StudentSubjectData.uid where StudentSubjectData.subjectId = '84cd2e3a-f898-4bbf-ade4-921ee59abe37'

select * from Students
select * from Subjects
select * from StudentSubjectData
INSERT into StudentSubjectData (subjectId,uid) values ('84cd2e3a-f898-4bbf-ade4-921ee59abe3','02000129453')

INSERT INTO Students (uid,firstName,middleName,lastName,section) VALUES ('02000129454','Jermaine','C','Marabe','BSCS 301');




DROP TABLE StudentSubjectData