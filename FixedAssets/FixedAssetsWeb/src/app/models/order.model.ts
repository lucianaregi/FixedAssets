import { OrderItem } from "./order-item.model";

export interface Order {
  id: number;
  userId: number;
  orderItems: OrderItem[];
  orderDate: string;
}

