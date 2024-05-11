-- Create table
create table PERSONA
(
  id_persona     NUMBER generated always as identity,
  nombre         VARCHAR2(100),
  genero         VARCHAR2(10),
  edad           NUMBER,
  identificacion VARCHAR2(20),
  direccion      VARCHAR2(200),
  telefono       VARCHAR2(20),
  estado         CHAR(1)
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Add comments to the columns 
comment on column PERSONA.id_persona
  is 'Identificador único de la persona';
comment on column PERSONA.nombre
  is 'Nombre de la persona';
comment on column PERSONA.genero
  is 'Género de la persona';
comment on column PERSONA.edad
  is 'Edad de la persona';
comment on column PERSONA.identificacion
  is 'Número de identificación de la persona';
comment on column PERSONA.direccion
  is 'Dirección de la persona';
comment on column PERSONA.telefono
  is 'Número de teléfono de la persona';
comment on column PERSONA.estado
  is 'Estado de la persona. A (Activo) I (Inactivo)';
-- Create/Recreate indexes 
create index IDX_PERSONA_IDENTIFICACION on PERSONA (IDENTIFICACION)
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table PERSONA
  add primary key (ID_PERSONA)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

-- Create table
create table CLIENTE
(
  id_cliente NUMBER generated always as identity,
  persona_id NUMBER,
  contraseña VARCHAR2(100),
  estado     VARCHAR2(20)
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Add comments to the columns 
comment on column CLIENTE.id_cliente
  is 'Identificador único del cliente';
comment on column CLIENTE.persona_id
  is 'Identificador de la persona asociada al cliente';
comment on column CLIENTE.contraseña
  is 'Contraseña del cliente';
comment on column CLIENTE.estado
  is 'Estado del cliente';
-- Create/Recreate indexes 
create index IDX_CLIENTE_PERSONAID on CLIENTE (PERSONA_ID)
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table CLIENTE
  add primary key (ID_CLIENTE)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table CLIENTE
  add constraint FK_CLIENTE_PERSONA foreign key (PERSONA_ID)
  references PERSONA (ID_PERSONA);


-- Create table
create table CUENTA
(
  numero_cuenta NUMBER not null,
  tipo_cuenta   VARCHAR2(50) not null,
  saldo_inicial NUMBER not null,
  estado        CHAR(1) not null,
  id_cliente    NUMBER not null
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Add comments to the columns 
comment on column CUENTA.numero_cuenta
  is 'Número de cuenta único';
comment on column CUENTA.tipo_cuenta
  is 'Tipo de cuenta';
comment on column CUENTA.saldo_inicial
  is 'Saldo inicial de la cuenta';
comment on column CUENTA.estado
  is 'Estado de la cuenta';
comment on column CUENTA.id_cliente
  is 'Cliente asociado a la cuenta';
-- Create/Recreate indexes 
create index IDX_CUENTA_NRO_CUENTA on CUENTA (ID_CLIENTE)
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table CUENTA
  add primary key (NUMERO_CUENTA)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table CUENTA
  add constraint FK_CUENTA_CLIENTE foreign key (ID_CLIENTE)
  references CLIENTE (ID_CLIENTE);

  -- Create table
create table MOVIMIENTOS
(
  id_movimiento   NUMBER generated always as identity,
  fecha           DATE not null,
  tipo_movimiento CHAR(1) not null,
  valor           NUMBER not null,
  saldo           NUMBER not null,
  numero_cuenta   NUMBER not null,
  fecha_registro  DATE not null,
  estado          CHAR(1)
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Add comments to the columns 
comment on column MOVIMIENTOS.id_movimiento
  is 'Identificador único del movimiento';
comment on column MOVIMIENTOS.fecha
  is 'Fecha del movimiento';
comment on column MOVIMIENTOS.tipo_movimiento
  is 'Tipo de movimiento (0 depósito, 1 retiro)';
comment on column MOVIMIENTOS.valor
  is 'Valor del movimiento';
comment on column MOVIMIENTOS.saldo
  is 'Saldo resultante después del movimiento';
comment on column MOVIMIENTOS.numero_cuenta
  is 'Numero de cuenta asociada al movimiento';
comment on column MOVIMIENTOS.fecha_registro
  is 'Fecha en que se registró la operación';
comment on column MOVIMIENTOS.estado
  is 'Estado del movimiento. P (Pendiente) C (Completado)';
-- Create/Recreate indexes 
create index IDX_MOVIMIENTO_FECHAREGISTRO on MOVIMIENTOS (FECHA_REGISTRO)
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
create index IDX_MOVIMIENTO_NRO_CUENTA on MOVIMIENTOS (NUMERO_CUENTA)
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table MOVIMIENTOS
  add primary key (ID_MOVIMIENTO)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table MOVIMIENTOS
  add constraint FK_MOVIMIENTO_CUENTA foreign key (NUMERO_CUENTA)
  references CUENTA (NUMERO_CUENTA);


--create user
CREATE USER API_DEVSU
  IDENTIFIED BY "a.123456" 
  DEFAULT TABLESPACE USERS
  TEMPORARY TABLESPACE TEMP
  PROFILE DEFAULT;
  
GRANT ALL PRIVILEGES TO API_DEVSU;