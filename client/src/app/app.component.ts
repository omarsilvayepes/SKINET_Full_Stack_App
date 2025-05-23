import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';
import { ShopService } from './core/services/shop.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
 
  private shopService=inject(ShopService);
  products:product[]=[];

  title = 'Skinet';

   ngOnInit(): void {

    this.shopService.getProducts().subscribe({
      next:response=>this.products=response.data,
      error:error=> console.log(error)
      //complete:()=> console.log('complete')
    })

  }
}
