CREATE TABLE Clients (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    surname NVARCHAR(50) NOT NULL,
    email NVARCHAR(50) NOT NULL,
    date_of_birth DATE NOT NULL,
    credit FLOAT NOT NULL DEFAULT 0.0,
    is_active BIT NOT NULL DEFAULT 1
);

CREATE TABLE Trainers (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    surname NVARCHAR(50) NOT NULL,
    specialization NVARCHAR(50) NOT NULL CHECK (specialization IN ('Yoga', 'Silovy', 'Cardio', 'Crossfit'))
);

CREATE TABLE Lessons (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(20) NOT NULL,
    date_and_time DATETIME NOT NULL,
    capacity INT NOT NULL,
    trainer_id INT NOT NULL FOREIGN KEY REFERENCES Trainers(id)
);

CREATE TABLE Bookings (
    id INT IDENTITY(1,1) PRIMARY KEY,
    client_id INT NOT NULL FOREIGN KEY REFERENCES Clients(id),
    lesson_id INT NOT NULL FOREIGN KEY REFERENCES Lessons(id),
    date_of_creation DATETIME DEFAULT GETDATE()
);

CREATE TABLE Logs (
    id INT IDENTITY(1,1) PRIMARY KEY,
    message NVARCHAR(255) NOT NULL,
    date DATETIME DEFAULT GETDATE(),
    type NVARCHAR(50) NOT NULL
);

GO
CREATE VIEW Lesson_Schedule AS
SELECT 
    L.id AS lesson_id,
    L.name AS lesson_name,
    L.date_and_time,
    L.capacity,
    T.name,
	T.surname,
    T.specialization
FROM Lessons L
JOIN Trainers T ON L.trainer_id = T.id;
GO

GO
CREATE VIEW Booking_Details AS
SELECT 
    B.id AS booking_id,
    C.name,
	C.surname,
    L.name AS lesson_name,
    L.date_and_time
FROM Bookings B
JOIN Clients C ON B.client_id = C.id
JOIN Lessons L ON B.lesson_id = L.id;
GO

INSERT INTO Trainers (name, surname, specialization) VALUES 
('Petr', 'Novák', 'Silovy'),
('Jana', 'Malá', 'Yoga');

INSERT INTO Clients (name, surname, email, date_of_birth, credit, is_active) VALUES
('Jan', 'Zákazník', 'jan@demo.cz', '1990-05-15', 500.0, 1),
('Eva', 'Pokusná', 'eva@demo.cz', '1995-12-20', 1200.50, 1);

INSERT INTO Lessons(name, date_and_time, capacity, trainer_id) VALUES 
('Ranní Jóga', '2025-05-01 08:00:00', 15, 2),
('Hardcore Kruháè', '2025-05-01 18:00:00', 10, 1);