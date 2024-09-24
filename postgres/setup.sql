CREATE DATABASE tickets;
    GRANT ALL PRIVILEGES ON DATABASE tickets TO postgres;

    \c tickets

    CREATE TABLE IF NOT EXISTS ticket
    (
        id            SERIAL PRIMARY KEY,
        ticketUid     uuid UNIQUE NOT NULL,
        username      VARCHAR(80) NOT NULL,
        flightnumber VARCHAR(20) NOT NULL,
        price         INT         NOT NULL,
        status        VARCHAR(20) NOT NULL CHECK (status IN ('PAID', 'CANCELED'))
    );

    GRANT ALL PRIVILEGES ON TABLE ticket TO postgres;
    GRANT ALL PRIVILEGES ON SEQUENCE ticket_id_seq TO postgres;

    CREATE DATABASE flights;
    GRANT ALL PRIVILEGES ON DATABASE flights TO postgres;

    \c flights

    CREATE TABLE IF NOT EXISTS airport
    (
        id      SERIAL PRIMARY KEY,
        name    VARCHAR(255),
        city    VARCHAR(255),
        country VARCHAR(255)
    );

    INSERT INTO airport (name, city, country) values ('Шереметьево', 'Москва', 'Россия');
    INSERT INTO airport (name, city, country) values ('Пулково', 'Санкт-Петербург', 'Россия');

    CREATE TABLE IF NOT EXISTS flight
    (
        id              SERIAL PRIMARY KEY,
        flightnumber          VARCHAR(20)              NOT NULL,
        datetime        TIMESTAMP WITH TIME ZONE NOT NULL,
        fromairportid INT REFERENCES airport (id),
        toairportid   INT REFERENCES airport (id),
        price           INT                      NOT NULL
    );

    INSERT INTO flight (flightnumber, datetime, fromairportid, toairportid, price)
        values ('AFL031', cast('2024-10-08 20:00:00' as timestamp with time zone), 2, 1, 1500);
    INSERT INTO flight (flightnumber, datetime, fromairportid, toairportid, price)
        values ('AFL032', cast('2024-10-08 20:00:00' as timestamp with time zone), 1, 2, 3000);

    GRANT ALL PRIVILEGES ON TABLE airport TO postgres;
    GRANT ALL PRIVILEGES ON SEQUENCE airport_id_seq TO postgres;
    GRANT ALL PRIVILEGES ON TABLE flight TO postgres;
    GRANT ALL PRIVILEGES ON SEQUENCE flight_id_seq TO postgres;

    CREATE DATABASE privileges;
    GRANT ALL PRIVILEGES ON DATABASE privileges TO postgres;

    \c privileges

    CREATE TABLE IF NOT EXISTS privilege
    (
        id       SERIAL PRIMARY KEY,
        username VARCHAR(80) NOT NULL UNIQUE,
        status   VARCHAR(80) NOT NULL DEFAULT 'BRONZE' CHECK (status IN ('BRONZE', 'SILVER', 'GOLD')),
        balance  INT
    );

    CREATE TABLE IF NOT EXISTS privilege_history
    (
        id             SERIAL PRIMARY KEY,
        privilege_id   INT REFERENCES privilege (id),
        ticket_uid     uuid        NOT NULL,
        datetime       TIMESTAMP   NOT NULL,
        balance_diff   INT         NOT NULL,
        operation_type VARCHAR(20) NOT NULL CHECK (operation_type IN ('FILL_IN_BALANCE', 'DEBIT_THE_ACCOUNT'))
    );

    GRANT ALL PRIVILEGES ON TABLE privilege TO postgres;
    GRANT ALL PRIVILEGES ON SEQUENCE privilege_id_seq TO postgres;
    GRANT ALL PRIVILEGES ON TABLE privilege_history TO postgres;
    GRANT ALL PRIVILEGES ON SEQUENCE privilege_history_id_seq TO postgres;
    
    CREATE DATABASE users;
    GRANT ALL PRIVILEGES ON DATABASE users TO postgres;

    \c users

    CREATE TABLE IF NOT EXISTS "user"
    (
        id     uuid UNIQUE NOT NULL,
        login    VARCHAR(255),
        password    VARCHAR(255)
    );

    INSERT INTO "user" (id, login, password) values ('a4c4df14-b8f8-4201-97ce-d389f5082bba', 'admin', 'admin');

    CREATE DATABASE stats;
    GRANT ALL PRIVILEGES ON DATABASE stats TO postgres;

    \c "stats"

    CREATE TABLE IF NOT EXISTS stat
    (
        id         SERIAL PRIMARY KEY,
        "text"     VARCHAR(255)
    );
