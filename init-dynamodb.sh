#!/bin/bash

echo "Configuração Completa do DynamoDB Local"

if curl -s http://localhost:8000 > /dev/null 2>&1; then
    echo "DynamoDB Local está rodando!"
else
    if docker run -d -p 8000:8000 --name dynamodb-local amazon/dynamodb-local > /dev/null 2>&1; then
        echo "DynamoDB Local iniciado!"
        sleep 3
    else
        echo "Erro ao iniciar DynamoDB Local"
        exit 1
    fi
fi

AWS_DIR="$HOME/.aws"
if [ ! -d "$AWS_DIR" ]; then
    mkdir -p "$AWS_DIR"
fi

CREDENTIALS_FILE="$AWS_DIR/credentials"
cat > "$CREDENTIALS_FILE" << EOF
[default]
aws_access_key_id=fakeAccessKey
aws_secret_access_key=fakeSecretKey
EOF

CONFIG_FILE="$AWS_DIR/config"
cat > "$CONFIG_FILE" << EOF
[default]
region = us-east-1
output = json
EOF

if aws dynamodb describe-table --table-name Pedidos --region us-east-1 --endpoint-url http://localhost:8000 > /dev/null 2>&1; then
    echo "Tabela 'Pedidos' já existe!"
else
    aws dynamodb create-table \
        --table-name Pedidos \
        --attribute-definitions AttributeName=IdPedido,AttributeType=N \
        --key-schema AttributeName=IdPedido,KeyType=HASH \
        --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 \
        --region us-east-1 \
        --endpoint-url http://localhost:8000 > /dev/null 2>&1

    if [ $? -eq 0 ]; then
        echo "Tabela 'Pedidos' criada com sucesso!"
    else
        echo "Erro ao criar tabela 'Pedidos'"
        exit 1
    fi
fi


