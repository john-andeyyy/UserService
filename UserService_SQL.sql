-- Create the database
CREATE DATABASE IF NOT EXISTS UserService;

-- Use the database
USE UserService;

-- Create 3RD_PARTY_ENDPOINT table
CREATE TABLE `3RD_PARTY_ENDPOINT` (
    `PARTY_ID` INT(11) NOT NULL AUTO_INCREMENT,
    `PARTY_API_ENDPOINT` VARCHAR(2700) NOT NULL,
    `PARTY_API_NAME` VARCHAR(2700) NOT NULL,
    `PARTY_METHOD` VARCHAR(2700) NOT NULL,
    `STATUS` CHAR(1) DEFAULT 'A',
    `PARTY_CODE` VARCHAR(100) NOT NULL,
    `SERVICE_NAME` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`PARTY_ID`)
) ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

-- Insert sample endpoints
INSERT INTO `3RD_PARTY_ENDPOINT` (
    `PARTY_API_ENDPOINT`,
    `PARTY_API_NAME`,
    `PARTY_METHOD`,
    `PARTY_CODE`,
    `SERVICE_NAME`
) VALUES 
    ('http://localhost:5002/api/Todo/UserTodo/', 'Get User Todo by User ID', 'GET', 'GetUserTodos', 'TodoService'),
    ('http://localhost:5002/api/Todo/NewTodo', 'Create User Todo', 'POST', 'NewTodo', 'TodoService'),
    ('http://localhost:5002/api/Todo/Update', 'Update User Todo', 'PUT', 'UpdateTodo', 'TodoService'),
    ('http://localhost:5002/api/Todo/TodoID/', 'Get Todo By Todo Id', 'GET', 'GetTodoById', 'TodoService'),
    ('http://localhost:5002/api/Todo/Delete/', 'Delete User Todo by Todo ID', 'DELETE', 'DeleteTodo', 'TodoService'),
    ('http://localhost:5002/api/Todo/IsCompleted', 'Mark Todo as Completed by Todo id with params', 'PUT', 'MarkAsCompleted', 'TodoService');

-- Select all from endpoints
SELECT * FROM `3RD_PARTY_ENDPOINT`;

-- Create users table
CREATE TABLE `users` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `LastName` VARCHAR(100) NOT NULL,
    `FirstName` VARCHAR(100) NOT NULL,
    `MiddleName` VARCHAR(100),
    `Email` VARCHAR(255) NOT NULL UNIQUE,
    `Phone` VARCHAR(50) NOT NULL,
    `Address` TEXT NOT NULL,
    `Username` VARCHAR(100) NOT NULL UNIQUE,
    `Password` VARCHAR(255) NOT NULL,
    `Status` CHAR(1) DEFAULT 'A',
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Insert sample users
INSERT INTO `users` (
    `LastName`, `FirstName`, `MiddleName`, `Email`, `Phone`, `Address`, `Username`, `Password`
) VALUES
    ('Dela Cruz', 'Juan', 'Reyes', 'juan.delacruz@example.com', '09171234567', '123 Manila St.', 'juandelacruz', 'password123'),
    ('Santos', 'Maria', 'Luz', 'maria.santos@example.com', '09281234567', '456 Makati Ave.', 'mariasantos', 'pass456'),
    ('Garcia', 'Jose', 'Luis', 'jose.garcia@example.com', '09391234567', '789 Quezon Blvd.', 'josegarcia', 'mysecurepass'),
    ('Reyes', 'Anna', '', 'anna.reyes@example.com', '09451234567', '101 Pasig Rd.', 'annareyes', '12345678'),
    ('Torres', 'Mark', 'Anthony', 'mark.torres@example.com', '09561234567', '202 Taguig Dr.', 'marktorres', 'letmein');
