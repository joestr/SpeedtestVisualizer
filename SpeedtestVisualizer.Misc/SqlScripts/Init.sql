create table `SpeedtestResults` (
    Id bigint primary key auto_increment,
    MeasuringTimestamp datetime,
    DownstreamBps bigint,
    UpstreamBps bigint
);
