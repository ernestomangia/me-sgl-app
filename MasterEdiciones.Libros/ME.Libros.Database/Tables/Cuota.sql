CREATE TABLE [dbo].[Cuota] (
    [Id]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [Numero]           INT             NOT NULL,
    [Estado]           INT             NULL,
    [FechaVencimiento] DATETIME        NOT NULL,
    [FechaCobro]       DATETIME        NULL,
    [DiasAtraso]       BIGINT          NULL,
    [Monto]            DECIMAL (18, 2) NOT NULL,
    [MontoCobro]       DECIMAL (18, 2) NULL,
    [Saldo]            DECIMAL (18, 2) NULL,
    [Interes]          DECIMAL (18, 2) NULL,
    [FechaAlta]        DATETIME        NOT NULL,
    [Venta_Id]         BIGINT          NOT NULL,
    CONSTRAINT [PK_dbo.Cuota] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Cuota_dbo.Venta_Venta_Id] FOREIGN KEY ([Venta_Id]) REFERENCES [dbo].[Venta] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Venta_Id]
    ON [dbo].[Cuota]([Venta_Id] ASC);

