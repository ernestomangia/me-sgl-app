CREATE TABLE [dbo].[Venta] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [FechaVenta]     DATETIME        NOT NULL,
    [FechaCobro]     DATETIME        NOT NULL,
    [Estado]         INT             NOT NULL,
    [CantidadCuotas] INT             NOT NULL,
    [MontoCuota]     DECIMAL (18, 2) NOT NULL,
    [MontoCalculado] DECIMAL (18, 2) NOT NULL,
    [MontoVendido]   DECIMAL (18, 2) NOT NULL,
    [MontoCobrado]   DECIMAL (18, 2) NOT NULL,
    [Saldo]          DECIMAL (18, 2) NOT NULL,
    [FechaAlta]      DATETIME        NOT NULL,
    [Cliente_Id]     BIGINT          NOT NULL,
    [Cobrador_Id]    BIGINT          NOT NULL,
    [PlanPago_Id]    BIGINT          NULL,
    [Vendedor_Id]    BIGINT          NULL,
    CONSTRAINT [PK_dbo.Venta] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Venta_dbo.Cliente_Cliente_Id] FOREIGN KEY ([Cliente_Id]) REFERENCES [dbo].[Cliente] ([Id]),
    CONSTRAINT [FK_dbo.Venta_dbo.Cobrador_Cobrador_Id] FOREIGN KEY ([Cobrador_Id]) REFERENCES [dbo].[Cobrador] ([Id]),
    CONSTRAINT [FK_dbo.Venta_dbo.PlanPago_PlanPago_Id] FOREIGN KEY ([PlanPago_Id]) REFERENCES [dbo].[PlanPago] ([Id]),
    CONSTRAINT [FK_dbo.Venta_dbo.Vendedor_Vendedor_Id] FOREIGN KEY ([Vendedor_Id]) REFERENCES [dbo].[Vendedor] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Vendedor_Id]
    ON [dbo].[Venta]([Vendedor_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PlanPago_Id]
    ON [dbo].[Venta]([PlanPago_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Cobrador_Id]
    ON [dbo].[Venta]([Cobrador_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Cliente_Id]
    ON [dbo].[Venta]([Cliente_Id] ASC);

