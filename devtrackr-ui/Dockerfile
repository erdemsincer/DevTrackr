# 1. Build aşaması (Vite ile frontend derleniyor)
FROM node:18-alpine AS builder

WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# 2. Nginx aşaması (build edilen dosyalar servis ediliyor)
FROM nginx:alpine

# ✅ NGINX config dosyasını burada override et
COPY nginx.conf /etc/nginx/conf.d/default.conf

# ✅ Derlenen React dosyalarını NGINX'e kopyala
COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
