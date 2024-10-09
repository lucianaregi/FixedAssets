import { OrderItem } from "./order-item.model";
import { UserAsset } from "./user-asset.model";

export interface Product {
  id: number;
  name: string;
  indexer: string;
  unitPrice: number;
  stock: number;
  tax: number;
  orderItems: OrderItem[];
  userAssets: UserAsset[];
}


