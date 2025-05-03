-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Хост: localhost:8889
-- Час створення: Трв 02 2025 р., 14:06
-- Версія сервера: 8.0.40
-- Версія PHP: 8.3.14

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База даних: `HospitalAppointmentSystem`
--

-- --------------------------------------------------------

--
-- Структура таблиці `Appointments`
--

CREATE TABLE `Appointments` (
  `AppointmentId` int NOT NULL,
  `PatientId` int DEFAULT NULL,
  `DoctorId` int DEFAULT NULL,
  `AppointmentDateTime` datetime DEFAULT NULL,
  `Notes` varchar(255) DEFAULT NULL,
  `Status` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп даних таблиці `Appointments`
--

INSERT INTO `Appointments` (`AppointmentId`, `PatientId`, `DoctorId`, `AppointmentDateTime`, `Notes`, `Status`) VALUES
(1, 1, 1, '2025-05-02 10:00:00', 'Щорічний огляд', 0),
(2, 2, 2, '2025-05-03 11:30:00', 'Консультація з приводу болю в грудях', 0),
(3, 3, 3, '2025-05-02 14:00:00', 'Головний біль, запаморочення', 1),
(4, 4, 4, '2025-05-09 09:00:00', 'Перевірка зору', 0),
(5, 5, 1, '2025-05-03 13:00:00', 'Застуда, кашель', 0),
(6, 6, 2, '2025-05-09 15:30:00', 'Підвищений тиск', 0),
(7, 7, 3, '2025-05-02 16:00:00', 'Болі в спині', 2),
(8, 8, 4, '2025-05-03 10:30:00', 'Проблеми із зором', 0),
(9, 1, 2, '2025-05-09 13:30:00', 'Консультація кардіолога', 0),
(10, 2, 3, '2025-05-02 11:00:00', 'Мігрень', 1),
(11, 3, 4, '2025-05-03 14:30:00', 'Обстеження очей', 0),
(12, 4, 1, '2025-05-09 09:30:00', 'Загальний огляд', 0),
(13, 5, 3, '2025-05-02 15:00:00', 'Болі в шиї', 2),
(14, 6, 4, '2025-05-03 09:30:00', 'Короткозорість', 0),
(15, 7, 1, '2025-05-09 10:30:00', 'Огляд', 0);

-- --------------------------------------------------------

--
-- Структура таблиці `Availabilities`
--

CREATE TABLE `Availabilities` (
  `AvailabilityId` int NOT NULL,
  `DoctorId` int DEFAULT NULL,
  `DayOfWeek` int DEFAULT NULL,
  `StartTime` time DEFAULT NULL,
  `EndTime` time DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп даних таблиці `Availabilities`
--

INSERT INTO `Availabilities` (`AvailabilityId`, `DoctorId`, `DayOfWeek`, `StartTime`, `EndTime`) VALUES
(1, 1, 1, '09:00:00', '17:00:00'),
(2, 1, 3, '09:00:00', '17:00:00'),
(3, 1, 5, '09:00:00', '17:00:00'),
(4, 2, 2, '08:00:00', '16:00:00'),
(5, 2, 4, '08:00:00', '16:00:00'),
(6, 3, 1, '10:00:00', '18:00:00'),
(7, 3, 4, '10:00:00', '18:00:00'),
(8, 4, 3, '08:00:00', '16:00:00'),
(9, 4, 5, '08:00:00', '16:00:00');

-- --------------------------------------------------------

--
-- Структура таблиці `Doctors`
--

CREATE TABLE `Doctors` (
  `DoctorId` int NOT NULL,
  `FirstName` varchar(100) DEFAULT NULL,
  `LastName` varchar(100) DEFAULT NULL,
  `Specialization` varchar(100) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп даних таблиці `Doctors`
--

INSERT INTO `Doctors` (`DoctorId`, `FirstName`, `LastName`, `Specialization`, `Email`, `Phone`) VALUES
(1, 'Марія', 'Коваленко', 'Терапевт', 'kovalenko@hospital.com', '+380501234567'),
(2, 'Олександр', 'Петренко', 'Кардіолог', 'petrenko@hospital.com', '+380502345678'),
(3, 'Ірина', 'Шевченко', 'Невролог', 'shevchenko@hospital.com', '+380503456789'),
(4, 'Андрій', 'Мельник', 'Офтальмолог', 'melnyk@hospital.com', '+380504567890');

-- --------------------------------------------------------

--
-- Структура таблиці `Patients`
--

CREATE TABLE `Patients` (
  `PatientId` int NOT NULL,
  `FirstName` varchar(100) DEFAULT NULL,
  `LastName` varchar(100) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `DateOfBirth` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп даних таблиці `Patients`
--

INSERT INTO `Patients` (`PatientId`, `FirstName`, `LastName`, `Email`, `Phone`, `DateOfBirth`) VALUES
(1, 'Наталія', 'Бондаренко', 'bondarenko@gmail.com', '+380671234567', '1985-05-15 00:00:00'),
(2, 'Василь', 'Іваненко', 'ivanenko@gmail.com', '+380672345678', '1990-10-20 00:00:00'),
(3, 'Олена', 'Сидоренко', 'sydorenko@gmail.com', '+380673456789', '1978-03-08 00:00:00'),
(4, 'Петро', 'Гончаренко', 'goncharenko@gmail.com', '+380674567890', '1982-12-30 00:00:00'),
(5, 'Юлія', 'Литвиненко', 'lytvynenko@gmail.com', '+380675678901', '1995-07-25 00:00:00'),
(6, 'Максим', 'Романенко', 'romanenko@gmail.com', '+380676789012', '1988-04-10 00:00:00'),
(7, 'Тетяна', 'Кириленко', 'kyrylenko@gmail.com', '+380677890123', '1975-09-05 00:00:00'),
(8, 'Володимир', 'Захарченко', 'zakharchenko@gmail.com', '+380678901234', '1980-01-15 00:00:00');

--
-- Індекси збережених таблиць
--

--
-- Індекси таблиці `Appointments`
--
ALTER TABLE `Appointments`
  ADD PRIMARY KEY (`AppointmentId`),
  ADD KEY `PatientId` (`PatientId`),
  ADD KEY `DoctorId` (`DoctorId`);

--
-- Індекси таблиці `Availabilities`
--
ALTER TABLE `Availabilities`
  ADD PRIMARY KEY (`AvailabilityId`),
  ADD KEY `DoctorId` (`DoctorId`);

--
-- Індекси таблиці `Doctors`
--
ALTER TABLE `Doctors`
  ADD PRIMARY KEY (`DoctorId`);

--
-- Індекси таблиці `Patients`
--
ALTER TABLE `Patients`
  ADD PRIMARY KEY (`PatientId`);

--
-- AUTO_INCREMENT для збережених таблиць
--

--
-- AUTO_INCREMENT для таблиці `Appointments`
--
ALTER TABLE `Appointments`
  MODIFY `AppointmentId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT для таблиці `Availabilities`
--
ALTER TABLE `Availabilities`
  MODIFY `AvailabilityId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT для таблиці `Doctors`
--
ALTER TABLE `Doctors`
  MODIFY `DoctorId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT для таблиці `Patients`
--
ALTER TABLE `Patients`
  MODIFY `PatientId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Обмеження зовнішнього ключа збережених таблиць
--

--
-- Обмеження зовнішнього ключа таблиці `Appointments`
--
ALTER TABLE `Appointments`
  ADD CONSTRAINT `appointments_ibfk_1` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`PatientId`),
  ADD CONSTRAINT `appointments_ibfk_2` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`DoctorId`);

--
-- Обмеження зовнішнього ключа таблиці `Availabilities`
--
ALTER TABLE `Availabilities`
  ADD CONSTRAINT `availabilities_ibfk_1` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`DoctorId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
