SET FOREIGN_KEY_CHECKS = 0;

CREATE TABLE material (
    id CHAR(36) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    enable TINYINT(1) NOT NULL DEFAULT 1,
    stock TINYINT(1) NOT NULL DEFAULT 0,
    stockAmount INT NULL DEFAULT NULL
);

CREATE TABLE type (
    id CHAR(36) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    enable TINYINT(1) NOT NULL DEFAULT 1
);

CREATE TABLE product (
    id CHAR(36) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    enable TINYINT(1) NOT NULL DEFAULT 1,
    typeId CHAR(36) NULL,
    CONSTRAINT fk_product_type FOREIGN KEY (typeId) REFERENCES type(id) ON DELETE SET NULL
);

CREATE TABLE product_material (
    p_id CHAR(36) NOT NULL,
    m_id CHAR(36) NOT NULL,
    PRIMARY KEY (p_id, m_id),
    CONSTRAINT fk_product_material_product FOREIGN KEY (p_id) REFERENCES product(id) ON DELETE CASCADE,
    CONSTRAINT fk_product_material_material FOREIGN KEY (m_id) REFERENCES material(id) ON DELETE CASCADE
);

CREATE TABLE option_table (
    id CHAR(36) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    m_id CHAR(36) NULL,
    type_id CHAR(36) NULL,
    product_id CHAR(36) NULL,
    CONSTRAINT fk_option_material FOREIGN KEY (m_id) REFERENCES material(id) ON DELETE SET NULL,
    CONSTRAINT fk_option_type FOREIGN KEY (type_id) REFERENCES type(id) ON DELETE SET NULL,
    CONSTRAINT fk_option_product FOREIGN KEY (product_id) REFERENCES product(id) ON DELETE SET NULL
);

SET FOREIGN_KEY_CHECKS = 1;
