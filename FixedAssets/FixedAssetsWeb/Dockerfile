# Etapa de build
FROM node:18 AS build

WORKDIR /app

COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build --prod

# Etapa de produção
FROM nginx:alpine

# Copie os arquivos do build para o diretório correto
COPY --from=build /app/dist/fixed-assets-web/browser /usr/share/nginx/html

# Copie a configuração do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
