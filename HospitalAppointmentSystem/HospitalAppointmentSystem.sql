-- Видалення існуючих таблиць якщо вони є
DROP TABLE IF EXISTS Appointments;
DROP TABLE IF EXISTS Doctors;
DROP TABLE IF EXISTS Patients;

-- Створення таблиці лікарів
CREATE TABLE Doctors (
DoctorId INTEGER PRIMARY KEY AUTOINCREMENT,
FirstName TEXT NOT NULL,
LastName TEXT NOT NULL,
Specialization TEXT NOT NULL
);

-- Створення таблиці пацієнтів
CREATE TABLE Patients (
PatientId INTEGER PRIMARY KEY AUTOINCREMENT,
FirstName TEXT NOT NULL,
LastName TEXT NOT NULL,
DateOfBirth DATE NOT NULL
);

-- Створення таблиці призначень
CREATE TABLE Appointments (
AppointmentId INTEGER PRIMARY KEY AUTOINCREMENT,
DoctorId INTEGER NOT NULL,
PatientId INTEGER NOT NULL,
AppointmentDateTime DATETIME NOT NULL,
Status TEXT NOT NULL,
Notes TEXT,
FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId),
FOREIGN KEY (PatientId) REFERENCES Patients(PatientId)
);

-- Додавання тестових даних для лікарів
INSERT INTO Doctors (FirstName, LastName, Specialization) VALUES
('Марія', 'Коваленко', 'Терапевт'),
('Олександр', 'Петренко', 'Кардіолог'),
('Ірина', 'Мельник', 'Невролог');

-- Додавання тестових даних для пацієнтів
INSERT INTO Patients (FirstName, LastName, DateOfBirth) VALUES
('Тетяна', 'Кириленко', '1990-05-15'),
('Наталія', 'Бондаренко', '1985-08-20'),
('Максим', 'Романенко', '1995-03-10');

-- Додавання тестових даних для призначень
INSERT INTO Appointments (DoctorId, PatientId, AppointmentDateTime, Status, Notes) VALUES
(1, 1, '2025-05-09 10:30:00', 'Scheduled', 'Первинний огляд'),
(2, 2, '2025-05-09 13:30:00', 'Scheduled', 'Планова консультація'),
(2, 3, '2025-05-09 15:30:00', 'Scheduled', 'Повторний прийом');