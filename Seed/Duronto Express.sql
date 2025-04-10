INSERT INTO Stations (StationName, Location) VALUES 
('Nagpur', 'Nagpur, MH'),
('Badnera', 'Amravati, MH'),
('Bhusaval', 'Jalgaon, MH'),
('Mumbai', 'Mumbai, MH');

INSERT INTO Trains (TrainName, TrainType, TotalSeats, RunningDays) VALUES 
('Duronto Express', 'Superfast', 500, 'Mon,Tue,Wed,Thu,Fri,Sat,Sun');

INSERT INTO ClassTypes (ClassName) VALUES 
('AC First Class'),
('AC 2 Tier'),
('AC 3 Tier'),
('Sleeper');

INSERT INTO TrainSchedules (TrainID, StationID, ArrivalTime, DepartureTime, SequenceOrder, Fair, DistanceFromSource)
VALUES (1, 1, '2025-04-09 06:00:00', '2025-04-09 06:10:00', 1, 0, 0);

INSERT INTO TrainSchedules (TrainID, StationID, ArrivalTime, DepartureTime, SequenceOrder, Fair, DistanceFromSource)
VALUES (1, 2, '2025-04-09 08:30:00', '2025-04-09 08:35:00', 2, 200, 150);

INSERT INTO TrainSchedules (TrainID, StationID, ArrivalTime, DepartureTime, SequenceOrder, Fair, DistanceFromSource)
VALUES (1, 3, '2025-04-09 11:00:00', '2025-04-09 11:10:00', 3, 400, 350);

INSERT INTO TrainSchedules (TrainID, StationID, ArrivalTime, DepartureTime, SequenceOrder, Fair, DistanceFromSource)
VALUES (1, 4, '2025-04-09 17:00:00', '2025-04-09 17:00:00', 4, 700, 800);

INSERT INTO SeatAvailabilities(TrainID, Date, ClassTypeID, RemainingSeats) VALUES
(1, '2025-04-09', 1, 10),
(1, '2025-04-09', 2, 20),
(1, '2025-04-09', 3, 30),
(1, '2025-04-09', 4, 100);