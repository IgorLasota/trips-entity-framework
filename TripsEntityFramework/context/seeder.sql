USE trips_app;
GO

-- Inserting sample data into the Client table
INSERT INTO Client (IdClient, FirstName, LastName, Email, Telephone, Pesel)
VALUES
(1, 'John', 'Doe', 'john.doe@example.com', '123-456-7890', '99010100123'),
(2, 'Jane', 'Smith', 'jane.smith@example.com', '123-456-7891', '99010200124'),
(3, 'Alice', 'Johnson', 'alice.johnson@example.com', '123-456-7892', '99010300125'),
(4, 'Bob', 'Williams', 'bob.williams@example.com', '123-456-7893', '99010400126'),
(5, 'Charlie', 'Brown', 'charlie.brown@example.com', '123-456-7894', '99010500127');

-- Inserting sample data into the Country table
INSERT INTO Country (IdCountry, Name)
VALUES
    (1, 'Poland'),
    (2, 'Germany'),
    (3, 'France'),
    (4, 'Italy'),
    (5, 'Spain');

-- Inserting sample data into the Trip table
INSERT INTO Trip (IdTrip, Name, Description, DateFrom, DateTo, MaxPeople)
VALUES
    (1, 'Trip to Warsaw', 'Explore the historical capital of Poland', '2023-06-01', '2023-06-05', 50),
    (2, 'Trip to Berlin', 'A cultural tour around Berlin', '2023-07-10', '2023-07-15', 40),
    (3, 'Trip to Paris', 'Romantic getaway in the city of love', '2023-08-15', '2023-08-20', 30),
    (4, 'Trip to Rome', 'Historical exploration of ancient Rome', '2023-09-05', '2023-09-10', 35),
    (5, 'Trip to Madrid', 'Discover the vibrant life of Madrid', '2023-10-01', '2023-10-06', 45);

-- Inserting sample data into the Client_Trip table
INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
VALUES
    (1, 1, '2023-05-20', '2023-05-21'),
    (2, 2, '2023-06-25', '2023-06-26'),
    (3, 3, '2023-07-30', NULL), -- Assume payment is pending
    (4, 4, '2023-08-10', '2023-08-11'),
    (5, 5, '2023-09-20', '2023-09-21');

-- Inserting sample data into the Country_Trip table
INSERT INTO Country_Trip (IdCountry, IdTrip)
VALUES
    (1, 1), -- Warsaw is in Poland
    (2, 2), -- Berlin is in Germany
    (3, 3), -- Paris is in France
    (4, 4), -- Rome is in Italy
    (5, 5); -- Madrid is in Spain
