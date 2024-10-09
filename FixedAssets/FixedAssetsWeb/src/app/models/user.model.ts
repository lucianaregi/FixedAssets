import { Order } from "./order.model";
import { UserAsset } from "./user-asset.model";

export interface User {
  id: number;
  name: string;
  cpf: string;
  balance: number;
  assets: UserAsset[];
  orders: Order[];
}

