CREATE TABLE [persons]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[Name] NVARCHAR (255) NOT NULL,
[Email] NVARCHAR (255) NOT NULL DEFAULT '('''')',
[Password] NVARCHAR (255) NOT NULL,
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_creator] INT NOT NULL DEFAULT '((0))',
[datLastLogin] DATETIME,
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
[Rights] INT NOT NULL DEFAULT '((0))',
[EmailPref] INT NOT NULL DEFAULT '((-1))',
[DisplayPref] INT NOT NULL DEFAULT '((0))'
);

CREATE TABLE [wishes]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[ID_creator] INT NOT NULL,
[name] NVARCHAR (255) NOT NULL,
[description] NTEXT NOT NULL DEFAULT '('''')',
[status] INT NOT NULL DEFAULT '((0))',
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[datCompleted] DATETIME,
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
CONSTRAINT [FK_wishes_persons] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE UNIQUE INDEX [IX_wishes] ON [wishes] ([name]);

CREATE TABLE [forum_groups]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[title] NVARCHAR (255) NOT NULL,
[ID_creator] INT NOT NULL,
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
CONSTRAINT [FK_forum_groups_persons] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE TABLE [forum_threads]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[title] NVARCHAR (255) NOT NULL,
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_creator] INT NOT NULL,
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_group] INT NOT NULL,
CONSTRAINT [FK_forum_threads_forum_groups] FOREIGN KEY ([ID_group]) REFERENCES [forum_groups] ([ID]),
CONSTRAINT [FK_forum_threads_persons] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE TABLE [forum_messages]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[description] NTEXT NOT NULL,
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_creator] INT NOT NULL,
[ID_thread] INT NOT NULL,
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
CONSTRAINT [FK_forum_messages_forum_threads] FOREIGN KEY ([ID_thread]) REFERENCES [forum_threads] ([ID]),
CONSTRAINT [FK_forum_messages_persons] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE TABLE [adventures]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[Title] NVARCHAR (255) NOT NULL,
[Description] NTEXT NOT NULL DEFAULT '('''')',
[Status] INT NOT NULL DEFAULT '((0))',
[ID_master] INT NOT NULL DEFAULT '((0))',
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_creator] INT NOT NULL DEFAULT '((0))',
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_thread] INT NOT NULL,
CONSTRAINT [FK_adventures_forum_threads] FOREIGN KEY ([ID_thread]) REFERENCES [forum_threads] ([ID]),
CONSTRAINT [FK_adventures_persons] FOREIGN KEY ([ID_master]) REFERENCES [persons] ([ID]),
CONSTRAINT [FK_adventures_persons1] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE UNIQUE INDEX [IX_adventures] ON [adventures] ([Title]);

CREATE TABLE [heroes]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[name] NVARCHAR (255) NOT NULL,
[profession] NVARCHAR (255) NOT NULL DEFAULT '(''unbekannt'')',
[ID_player] INT NOT NULL,
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_creator] INT NOT NULL DEFAULT '((0))',
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
CONSTRAINT [FK_heroes_persons] FOREIGN KEY ([ID_player]) REFERENCES [persons] ([ID]),
CONSTRAINT [FK_heroes_persons1] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE TABLE [log]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[description] NTEXT NOT NULL,
[logType] INT NOT NULL DEFAULT '((0))',
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_creator] INT NOT NULL,
[name] NTEXT NOT NULL DEFAULT '('''')',
CONSTRAINT [FK_errors_persons] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE TABLE [meetings]
(
[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[datMeeting] DATETIME NOT NULL,
[ID_adventure] INT NOT NULL,
[datCreated] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_creator] INT NOT NULL DEFAULT '((0))',
[datModified] DATETIME NOT NULL DEFAULT (datetime('now')),
[ID_thread] INT NOT NULL,
CONSTRAINT [FK_meetings_adventures] FOREIGN KEY ([ID_adventure]) REFERENCES [adventures] ([ID]),
CONSTRAINT [FK_meetings_forum_threads] FOREIGN KEY ([ID_thread]) REFERENCES [forum_threads] ([ID]),
CONSTRAINT [FK_meetings_persons] FOREIGN KEY ([ID_creator]) REFERENCES [persons] ([ID])
);

CREATE INDEX [IX_meetings] ON [meetings] ([datMeeting]);

CREATE TABLE [rel_heroes_adventures]
(
[ID_hero] INT NOT NULL,
[ID_adventure] INT NOT NULL,
CONSTRAINT [PK_rel_heroes_adventures] PRIMARY KEY ([ID_hero] ,[ID_adventure]),
CONSTRAINT [FK_rel_heroes_adventures_adventures] FOREIGN KEY ([ID_adventure]) REFERENCES [adventures] ([ID]),
CONSTRAINT [FK_rel_heroes_adventures_heroes] FOREIGN KEY ([ID_hero]) REFERENCES [heroes] ([ID])
);

CREATE TABLE [rel_persons_meetings]
(
[ID_person] INT NOT NULL,
[ID_meeting] INT NOT NULL,
[status] INT NOT NULL DEFAULT '((1))',
CONSTRAINT [PK_rel_persons_meetings] PRIMARY KEY ([ID_person] ,[ID_meeting]),
CONSTRAINT [FK_rel_persons_meetings_meetings] FOREIGN KEY ([ID_meeting]) REFERENCES [meetings] ([ID]),
CONSTRAINT [FK_rel_persons_meetings_persons] FOREIGN KEY ([ID_person]) REFERENCES [persons] ([ID])
);

CREATE TABLE [rel_persons_threads]
(
[ID_thread] INT NOT NULL,
[ID_person] INT NOT NULL,
CONSTRAINT [PK_rel_persons_threads] PRIMARY KEY ([ID_thread] ,[ID_person]),
CONSTRAINT [FK_rel_persons_threads_forum_threads] FOREIGN KEY ([ID_thread]) REFERENCES [forum_threads] ([ID]),
CONSTRAINT [FK_rel_persons_threads_persons] FOREIGN KEY ([ID_person]) REFERENCES [persons] ([ID])
);


CREATE VIEW [MeetingPersons] AS SELECT     persons.Name, persons.Email, rel_persons_meetings.status, persons.ID, meetings.ID AS ID_meeting, persons.datLastLogin,
                       persons.ID_creator, persons.datCreated, persons.datModified, persons.Rights, persons.EmailPref, persons.DisplayPref
FROM         persons CROSS JOIN
                      meetings LEFT OUTER JOIN
                      rel_persons_meetings ON persons.ID = rel_persons_meetings.ID_person AND 
                      rel_persons_meetings.ID_meeting = meetings.ID
WHERE     (persons.ID <> 0)
;


CREATE VIEW [LogEntrys]
AS
SELECT     [log].*, datCreated AS datModified
FROM         [log]
;

/* Required default Values */

INSERT INTO persons (ID, Name, Email, Password, ID_creator, Rights, EmailPref, DisplayPref)
VALUES (0, 'keiner', 'Diesen Benutzer nicht löschen', 'Keine Anmeldung', 0, 0, -1, 0);
INSERT INTO persons (ID, Name, Email, Password, ID_creator, Rights, EmailPref, DisplayPref)
VALUES (1, 'admin', 'no-mail@nowhere', '6Pl/upEE0epQR5SObftn+s2fW3M=', 0, 1, 0, 0);

INSERT INTO [forum_groups] (ID, title, ID_creator) VALUES (0,'Termin-Diskussionen',0);
INSERT INTO [forum_groups] (ID, title, ID_creator) VALUES (2,'Allgemeine Diskussionen',0);
INSERT INTO [forum_groups] (ID, title, ID_creator) VALUES (3,'Fun',0);
INSERT INTO [forum_groups] (ID, title, ID_creator) VALUES (4,'Regeln',0);
INSERT INTO [forum_groups] (ID, title, ID_creator) VALUES (5,'Abenteuer',0);

INSERT INTO forum_threads (ID, title, ID_creator, ID_group) VALUES (0, 'Noch keines', 0, 5);

INSERT INTO adventures(ID,Title, Description, Status, ID_master, ID_creator, ID_thread)
VALUES (0, 'noch keines', 'noch kein Abenteuer zugewiesen', 0, 0, 0, 0);

