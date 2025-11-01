create database db_Student_Gradebook_System
go

use db_Student_Gradebook_System
go

create table [User](
	UserID				int PRIMARY KEY identity(1,1),
	UserName			nvarchar(50) not null ,
	Email				nvarchar(50) unique not null,
	[Password]			nvarchar(50) unique not null,
	UserRole			nvarchar(20) check(UserRole in ('Teacher','Student'))

)
INSERT INTO [User] (UserName, Email, [Password], UserRole) VALUES
('hi','hi@','h','Student');
--('shino','shio@','shin','Teacher'),
--('hamo','hamo@','ham','Student'),


create table Course(
	CourseID 			INT PRIMARY KEY identity(1,1),
	CourseName 			nvarchar(50) not null ,
	TeacherID 			int ,
	FOREIGN KEY (TeacherID) REFERENCES [User](UserID)
)
INSERT INTO Course (CourseName, TeacherID) VALUES
('How to eat', 1),
('Do you know how to sleep', 1);

create table Enrollment(
	EnrollmentID		int Primary Key identity(1,1),
	UserID				INT REFERENCES [User](UserID),
	CourseID			INT REFERENCES Course(CourseID),
) 
INSERT INTO Enrollment (UserID, CourseID) VALUES
--(2, 1),
--(2, 2),
(1003, 2);

create table Grade(
	GradeID				int Primary Key identity(1,1),
	EnrollmentID 		INT REFERENCES Enrollment(EnrollmentID),
	AssignmentName 		NVARCHAR(50) NOT NULL,
	Score				decimal,
	MaxScore			decimal,
)

INSERT INTO Grade (EnrollmentID, AssignmentName, Score, MaxScore) VALUES
(1003, 'Dream Control', 95, 100),
(1, 'Eating Assignment', 85, 100),
(1, 'Chewing Practice', 90, 100),
(2, 'Sleeping Basics', 88, 100),
(2, 'Dream Control', 95, 100);


drop table [User]
drop table Enrollment
drop table Grade
drop table Course


