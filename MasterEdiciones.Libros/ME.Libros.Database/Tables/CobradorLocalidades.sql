CREATE TABLE [dbo].[CobradorLocalidades] (
    [Cobrador_Id]  BIGINT NOT NULL,
    [Localidad_Id] BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.CobradorLocalidades] PRIMARY KEY CLUSTERED ([Cobrador_Id] ASC, [Localidad_Id] ASC),
    CONSTRAINT [FK_dbo.CobradorLocalidades_dbo.Cobrador_Cobrador_Id] FOREIGN KEY ([Cobrador_Id]) REFERENCES [dbo].[Cobrador] ([Id]),
    CONSTRAINT [FK_dbo.CobradorLocalidades_dbo.Localidad_Localidad_Id] FOREIGN KEY ([Localidad_Id]) REFERENCES [dbo].[Localidad] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Localidad_Id]
    ON [dbo].[CobradorLocalidades]([Localidad_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Cobrador_Id]
    ON [dbo].[CobradorLocalidades]([Cobrador_Id] ASC);

