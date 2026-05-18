
CREATE DATABASE IF NOT EXISTS padaria_autenticacao;
CREATE DATABASE IF NOT EXISTS padaria_produtos;
CREATE DATABASE IF NOT EXISTS padaria_pedidos;

-- BANCO: padaria_autenticacao

USE padaria_autenticacao;

CREATE TABLE IF NOT EXISTS Usuarios (
    id            CHAR(36)     NOT NULL,
    nome_usuario  VARCHAR(50)  NOT NULL,
    nome          VARCHAR(100) NOT NULL,
    email         VARCHAR(150) NOT NULL,
    senha_hash    VARCHAR(255) NOT NULL,
    telefone      VARCHAR(20)      NULL,
    perfil        VARCHAR(30)  NOT NULL DEFAULT 'Cliente',
    criado_em     DATETIME     NOT NULL DEFAULT NOW(),
    atualizado_em DATETIME         NULL,
    ativo         TINYINT(1)   NOT NULL DEFAULT 1,

    CONSTRAINT pk_usuarios PRIMARY KEY (id),
    CONSTRAINT uq_usuarios_email UNIQUE (email),
    CONSTRAINT uq_usuarios_nome_usuario UNIQUE (nome_usuario)
);


-- BANCO: padaria_produtos

USE padaria_produtos;

CREATE TABLE IF NOT EXISTS Categorias (
    id          CHAR(36)     NOT NULL,
    nome        VARCHAR(100) NOT NULL,
    descricao   VARCHAR(255)     NULL,
    ativa       TINYINT(1)   NOT NULL DEFAULT 1,
    criado_em   DATETIME     NOT NULL DEFAULT NOW(),

    CONSTRAINT pk_categorias PRIMARY KEY (id),
    CONSTRAINT uq_categorias_nome UNIQUE (nome)
);

CREATE TABLE IF NOT EXISTS Produtos (
    id            CHAR(36)      NOT NULL,
    nome          VARCHAR(150)  NOT NULL,
    descricao     VARCHAR(500)      NULL,
    preco         DECIMAL(10,2) NOT NULL,
    estoque       INT           NOT NULL DEFAULT 0,
    url_imagem    VARCHAR(500)      NULL,
    disponivel    TINYINT(1)    NOT NULL DEFAULT 1,
    categoria_id  CHAR(36)      NOT NULL,
    criado_em     DATETIME      NOT NULL DEFAULT NOW(),
    atualizado_em DATETIME          NULL,

    CONSTRAINT pk_produtos PRIMARY KEY (id),
    CONSTRAINT fk_produtos_categoria FOREIGN KEY (categoria_id)
        REFERENCES Categorias(id),
    INDEX idx_produtos_nome (nome),
    INDEX idx_produtos_categoria_id (categoria_id)
);

-- BANCO: padaria_pedidos

USE padaria_pedidos;

CREATE TABLE IF NOT EXISTS Pedidos (
    id               CHAR(36)      NOT NULL,
    usuario_id       CHAR(36)          NULL,
    nome_cliente     VARCHAR(150)  NOT NULL,
    email_cliente    VARCHAR(150)      NULL,
    telefone_cliente VARCHAR(20)       NULL,
    tipo             VARCHAR(20)   NOT NULL,
    status           VARCHAR(20)   NOT NULL,
    total            DECIMAL(10,2) NOT NULL,
    observacoes      VARCHAR(500)      NULL,
    criado_em        DATETIME      NOT NULL DEFAULT NOW(),
    atualizado_em    DATETIME          NULL,

    CONSTRAINT pk_pedidos PRIMARY KEY (id),
    INDEX idx_pedidos_usuario_id (usuario_id),
    INDEX idx_pedidos_status (status),
    INDEX idx_pedidos_criado_em (criado_em)
);

CREATE TABLE IF NOT EXISTS ItensPedido (
    id             CHAR(36)      NOT NULL,
    pedido_id      CHAR(36)      NOT NULL,
    produto_id     CHAR(36)      NOT NULL,
    nome_produto   VARCHAR(150)  NOT NULL,
    quantidade     INT           NOT NULL,
    preco_unitario DECIMAL(10,2) NOT NULL,
    subtotal       DECIMAL(10,2) NOT NULL,

    CONSTRAINT pk_itens_pedido PRIMARY KEY (id),
    CONSTRAINT fk_itens_pedido FOREIGN KEY (pedido_id)
        REFERENCES Pedidos(id) ON DELETE CASCADE,
    INDEX idx_itens_pedido_id (pedido_id)
);