# Stage 1: Build
FROM node:20-alpine AS build
WORKDIR /app

# Copiar arquivos de dependências
COPY package*.json ./
RUN npm ci

# Copiar código fonte e compilar
COPY . .
RUN npm run build

# Stage 2: Serve com nginx
FROM nginx:alpine
COPY --from=build /app/dist/bmptecbank-frontend/browser /usr/share/nginx/html

# Copiar configuração customizada do nginx (opcional)
RUN echo 'server { \
    listen 80; \
    server_name localhost; \
    root /usr/share/nginx/html; \
    index index.html; \
    location / { \
        try_files $uri $uri/ /index.html; \
    } \
}' > /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

