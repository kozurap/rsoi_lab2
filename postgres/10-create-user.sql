-- file: 10-create-user.sql
CREATE ROLE postgres WITH PASSWORD 'postgres';
ALTER ROLE postgres WITH LOGIN;